using System.ComponentModel.DataAnnotations;

namespace BenefitsManager.Models
{
    public class Payment
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Valor inicial")]
        [Required(ErrorMessage = "Digite o valor inicial.")]
        public float InitialValue { get; set; }

        [Display(Name = "Valor com desconto")]
        public float FinalValue { get; set; }

        public int TaxpayerId { get; set; }
        public Taxpayer Taxpayer { get; set; }

        public int? BenefitId { get; set; }
        public Benefit? Benefit { get;    set; }
    }
}
