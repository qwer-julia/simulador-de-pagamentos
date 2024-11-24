using System.ComponentModel.DataAnnotations;

namespace BenefitsManager.Models
{
    public class Benefit
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Digite o nome do benefício.")]
        public string Name { get; set; }

        [Display(Name = "Percentual de desconto")]
        [Required(ErrorMessage = "Digite o percentual de desconto.")]
        public float DiscountPercentage { get; set; }

        public ICollection<TaxpayerBenefit>? TaxpayerBenefits { get; set; }

    }
}
