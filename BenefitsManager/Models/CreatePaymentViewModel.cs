using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BenefitsManager.Models
{
    public class CreatePaymentViewModel
    {
        [Required]
        public int TaxpayerId { get; set; }

        public int? BenefitId { get; set; }

        [Required]
        public float InitialValue { get; set; }

        public string TaxpayerName { get; set; } 

        public List<SelectListItem>? Benefits { get; set; } 
    }
}
