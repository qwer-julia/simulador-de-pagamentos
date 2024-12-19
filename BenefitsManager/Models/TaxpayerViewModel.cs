using System.ComponentModel.DataAnnotations;

namespace BenefitsManager.Models
{
    public class TaxpayerViewModel
    {
        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(18, ErrorMessage = "O CNPJ deve ter no máximo 18 caracteres com máscara.")]
        public string Cnpj { get; set; }

        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Razão Social")]
        [Required(ErrorMessage = "Digite a Razão Social.")]
        public string CompanyName { get; set; }

        [Display(Name = "Data de abertura")]
        [Required(ErrorMessage = "Digite a data de abertura.")]
        public DateOnly OpeningDate { get; set; }

        [Display(Name = "Regime de tributação")]
        [Required(ErrorMessage = "Digite o regime de tributação.")]
        public string TaxationRegime { get; set; }

        public ICollection<TaxpayerBenefit>? TaxpayerBenefits { get; set; }
    }
}
