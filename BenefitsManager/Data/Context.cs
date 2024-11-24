using BenefitsManager.Models;
using Microsoft.EntityFrameworkCore;

namespace BenefitsManager.Data
{
    public class Context : DbContext
    {
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<Taxpayer> Taxpayers { get; set; }
        public DbSet<TaxpayerBenefit> TaxpayerBenefits { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public Context(DbContextOptions<Context> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaxpayerBenefit>()
                .HasKey(tb => new { tb.TaxpayerId, tb.BenefitId });

            modelBuilder.Entity<TaxpayerBenefit>()
                .HasOne(tb => tb.Taxpayer)
                .WithMany(t => t.TaxpayerBenefits)
                .HasForeignKey(tb => tb.TaxpayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaxpayerBenefit>()
                .HasOne(tb => tb.Benefit)
                .WithMany(b => b.TaxpayerBenefits)
                .HasForeignKey(tb => tb.BenefitId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Taxpayer)
                .WithMany()
                .HasForeignKey(p => p.TaxpayerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Benefit)
                .WithMany()
                .HasForeignKey(p => p.BenefitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Benefit>().HasData(new Benefit
            {
                Id = 1,
                Name = "Sem Benefício",
                DiscountPercentage = 0
            });

        }
    }
}
