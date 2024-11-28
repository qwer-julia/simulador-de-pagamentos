using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BenefitsManager.Data;
using BenefitsManager.Models;
using static System.Net.Mime.MediaTypeNames;

namespace BenefitsManager.Controllers
{
    public class BenefitsController : Controller
    {
        private readonly Context _context;

        public BenefitsController(Context context)
        {
            _context = context;
        }

        // GET: Benefits
        public async Task<IActionResult> Index()
        {
            var benefits = await _context.Benefits
                .Where(b => b.Id != 1)
                .GroupJoin(_context.TaxpayerBenefits,
                    b => b.Id,              
                    tb => tb.BenefitId,    
                    (b, tb) => new
                    {
                        b.Id,
                        BenefitName = b.Name,
                        b.DiscountPercentage,
                        TaxpayerCount = tb.Count() 
                    })
                .OrderBy(b => b.Id)
                .ToListAsync(); 

            return View(benefits);
        }

        // GET: Benefits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benefit = await _context.Benefits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (benefit == null)
            {
                return NotFound();
            }

            return View(benefit);
        }

        // GET: Benefits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Benefits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DiscountPercentage")] Benefit benefit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(benefit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
           
            return View(benefit);
        }

        // GET: Benefits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benefit = await _context.Benefits.FindAsync(id);
            if (benefit == null)
            {
                return NotFound();
            }
            return View(benefit);
        }

        // POST: Benefits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DiscountPercentage")] Benefit benefit)
        {
            if (id != benefit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(benefit);
                    await _context.SaveChangesAsync();
                    //verificar se será necessário atualizar também os cadastros de pagamento ja vinculados à esse contribuinte
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BenefitExists(benefit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(benefit);
        }

        // GET: Benefits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benefit = await _context.Benefits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (benefit == null)
            {
                return NotFound();
            }

            return View(benefit);
        }

        // POST: Benefits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var benefit = await _context.Benefits.FindAsync(id);
            if (benefit != null)
            {
                _context.Benefits.Remove(benefit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BenefitExists(int id)
        {
            return _context.Benefits.Any(e => e.Id == id);
        }
    }
}
