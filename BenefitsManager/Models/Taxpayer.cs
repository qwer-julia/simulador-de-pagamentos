using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BenefitsManager.Models
{
    public class Taxpayer
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "Digite o CNPJ.")]
        public long Cnpj { get; set; }

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
        [NotMapped]

        [Required(ErrorMessage = "É necessário selecionar pelo menos 1 benefício")]
        public List<int> SelectedBenefits { get; set; }
    }
}
