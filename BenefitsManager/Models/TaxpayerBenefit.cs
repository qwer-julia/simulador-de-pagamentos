using System.ComponentModel.DataAnnotations;

namespace BenefitsManager.Models
{
    public class TaxpayerBenefit
    {
        public int TaxpayerId { get; set; }
        public Taxpayer Taxpayer { get; set; }

        public int BenefitId { get; set; }
        public Benefit Benefit { get; set; }

    }
}
