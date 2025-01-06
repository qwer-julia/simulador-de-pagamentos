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
        [Range(0, 100, ErrorMessage = "O desconto deve estar entre 0 e 100")]
        [Required(ErrorMessage = "Digite o percentual de desconto.")]
        public float DiscountPercentage { get; set; }

        public ICollection<TaxpayerBenefit>? TaxpayerBenefits { get; set; }

    }
}
