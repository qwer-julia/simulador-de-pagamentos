using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BenefitsManager.Data;
using BenefitsManager.Models;
using AutoMapper;
using System.Drawing;

namespace BenefitsManager.Controllers
{
    public class TaxpayersController : Controller
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public TaxpayersController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Taxpayers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Taxpayers.Include(t => t.TaxpayerBenefits).ToListAsync());
        }

        // GET: Taxpayers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxpayer = await _context.Taxpayers
                .Include(t => t.TaxpayerBenefits)
                .ThenInclude(tb => tb.Benefit)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (taxpayer == null)
            {
                return NotFound();
            }

            return View(taxpayer);
        }

        // GET: Taxpayers/Create
        public async Task<IActionResult> Create()

        {
            var benefits = await _context.Benefits.ToListAsync();
            ViewBag.Benefits = benefits;
            return View();
        }

        // POST: Taxpayers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cnpj,CompanyName,OpeningDate,TaxationRegime")] Taxpayer taxpayer, List<int> SelectedBenefits)
        {
            ViewBag.Benefits = _context.Benefits.ToList();

            if (SelectedBenefits == null || !SelectedBenefits.Any())
            {
                ModelState.AddModelError("SelectedBenefits", "Selecione pelo menos um benefício.");

                ViewBag.Benefits = _context.Benefits.ToList();
                return View(taxpayer);
            }

            if (CnpjAlreadyInUse(taxpayer.Cnpj, taxpayer.Id))
            {
                ModelState.AddModelError("Cnpj", "CNPJ já cadastrado");
                return View(taxpayer);
            }

            _context.Add(taxpayer);
            await _context.SaveChangesAsync();

            foreach (var benefitId in SelectedBenefits)
            {
                var taxpayerBenefit = new TaxpayerBenefit
                {
                    TaxpayerId = taxpayer.Id,
                    BenefitId = benefitId
                };

                _context.TaxpayerBenefits.Add(taxpayerBenefit);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Taxpayers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxpayer = await _context.Taxpayers
            .Include(t => t.TaxpayerBenefits)
            .ThenInclude(tb => tb.Benefit)
            .FirstOrDefaultAsync(t => t.Id == id);

            var benefits = await _context.Benefits.ToListAsync();
            ViewBag.Benefits = benefits;


            if (taxpayer == null)
            {
                return NotFound();
            }
            return View(taxpayer);
        }

        // POST: Taxpayers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cnpj,CompanyName,OpeningDate,TaxationRegime")] Taxpayer taxpayer, List<int> SelectedBenefits)
        {
            var benefits = await _context.Benefits.ToListAsync();
            ViewBag.Benefits = benefits;

            var taxpayerFromDb = await _context.Taxpayers
            .Include(t => t.TaxpayerBenefits)
            .ThenInclude(tb => tb.Benefit)
            .FirstOrDefaultAsync(t => t.Id == id);

            if (id != taxpayer.Id)
            {
                return NotFound();
            }

            if (CnpjAlreadyInUse(taxpayer.Cnpj, id))
            {
                ModelState.AddModelError("Cnpj", "CNPJ já cadastrado");
                return View(taxpayer);
            }

            if (SelectedBenefits == null || !SelectedBenefits.Any())
            {
                ModelState.AddModelError("SelectedBenefits", "Selecione pelo menos um benefício.");
                return View(taxpayer);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _mapper.Map(taxpayer, taxpayerFromDb);

                    var existingBenefitIds = taxpayerFromDb.TaxpayerBenefits.Select(tb => tb.BenefitId).ToList();
                    var benefitsToRemove = taxpayerFromDb?.TaxpayerBenefits?
                    .Where(tb => !SelectedBenefits.Contains(tb.BenefitId))
                    .ToList();

                    var benefitsToAdd = SelectedBenefits
                    .Where(id => !existingBenefitIds.Contains(id))
                    .Select(id => new TaxpayerBenefit { TaxpayerId = taxpayer.Id, BenefitId = id })
                    .ToList();

                    if (benefitsToRemove != null)
                    {
                        _context.TaxpayerBenefits.RemoveRange(benefitsToRemove);
                        await _context.SaveChangesAsync();
                    }

                    _context.TaxpayerBenefits.AddRange(benefitsToAdd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxpayerExists(taxpayer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Taxpayers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxpayer = await _context.Taxpayers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (taxpayer == null)
            {
                return NotFound();
            }

            return View(taxpayer);
        }

        // POST: Taxpayers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taxpayer = await _context.Taxpayers.FindAsync(id);
            if (taxpayer != null)
            {
                _context.Taxpayers.Remove(taxpayer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaxpayerExists(int id)
        {
            return _context.Taxpayers.Any(e => e.Id == id);
        }

        private bool CnpjAlreadyInUse(long cnpj, int id)
        {
            return _context.Taxpayers.Any(t => t.Cnpj == cnpj && t.Id != id);
        }        
    }
}
