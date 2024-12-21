using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BenefitsManager.Data;
using BenefitsManager.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace BenefitsManager.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly Context _context;

        public PaymentsController(Context context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // POST: Payments/SearchTaxpayer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchTaxpayer(long cnpj)
        {
            var taxpayer = await _context.Taxpayers
                .Include(t => t.TaxpayerBenefits)
                .FirstOrDefaultAsync(t => t.Cnpj == cnpj);

            if (taxpayer == null)
            {
                TempData["Error"] = "CNPJ não encontrado. Verifique e tente novamente.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create", new { taxpayerId = taxpayer.Id });
        }

        // GET: Payments/Create
        public async Task<IActionResult> Create(int taxpayerId)
        {
            var taxpayer = await _context.Taxpayers
                .FirstOrDefaultAsync(t => t.Id == taxpayerId);

            if (taxpayer == null)
            {
                return NotFound();
            }

            var viewModel = new CreatePaymentViewModel
            {
                TaxpayerId = taxpayer.Id,
                TaxpayerName = taxpayer.CompanyName,
                Benefits = await _context.Benefits
                    .Where(b => b.TaxpayerBenefits.Any(tb => tb.TaxpayerId == taxpayerId))
                    .Select(b => new SelectListItem
                    {
                        Value = b.Id.ToString(),
                        Text = b.Name
                    })
                    .ToListAsync()
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentViewModel model)
        {            
            var benefit = await _context.Benefits
                .FirstOrDefaultAsync(m => m.Id == model.BenefitId);

            var finalValue = model.InitialValue;

            if (benefit != null)
            {
                finalValue = model.InitialValue * (1 - (benefit.DiscountPercentage / 100));
            }

            var payment = new Payment
            {
                TaxpayerId = model.TaxpayerId,
                BenefitId = model.BenefitId,
                InitialValue = model.InitialValue,
                FinalValue = finalValue,
                Benefit = benefit,
                Taxpayer = await _context.Taxpayers.FirstOrDefaultAsync(m => m.Id == model.TaxpayerId)
            };

            HttpContext.Session.SetString("PaymentData", JsonConvert.SerializeObject(payment));
            return RedirectToAction("ConfirmPayment");
        }
        public IActionResult ConfirmPayment()
        {
            var paymentJson = HttpContext.Session.GetString("PaymentData");
            var payment = JsonConvert.DeserializeObject<Payment>(paymentJson);

            return View(payment);
        }
    }
}
