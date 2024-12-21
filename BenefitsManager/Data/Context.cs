using BenefitsManager.Models;
using Microsoft.EntityFrameworkCore;

namespace BenefitsManager.Data
{
    public class Context : DbContext
    {
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<Taxpayer> Taxpayers { get; set; }
        public DbSet<TaxpayerBenefit> TaxpayerBenefits { get; set; }

        public Context(DbContextOptions<Context> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Taxpayer>()
            .HasIndex(t => t.Cnpj)
            .IsUnique();

            modelBuilder.Entity<Taxpayer>()
            .HasMany(t => t.TaxpayerBenefits)
            .WithOne(tb => tb.Taxpayer)
            .HasForeignKey(tb => tb.TaxpayerId)
            .IsRequired();

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

            modelBuilder.Entity<Benefit>().HasData(new Benefit
            {
                Id = 1,
                Name = "Desconto zerado",
                DiscountPercentage = 0
            });

        }
    }
}
