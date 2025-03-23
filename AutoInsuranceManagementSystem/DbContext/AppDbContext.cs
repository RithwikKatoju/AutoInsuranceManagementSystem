using AutoInsuranceManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoInsuranceManagementSystem.DbContext
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<UserEntityModel>(options)
    {
        public DbSet<UserEntityModel> UsersEntity {  get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PolicyEntityModel>()
                .OwnsOne(p => p.VehicleDetails, vd =>
                {
                    vd.Property(v => v.Make).HasColumnName("VehicleMake");
                    vd.Property(v => v.Model).HasColumnName("VehicleModel");
                    vd.Property(v => v.Year).HasColumnName("VehicleYear");
                });

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<PolicyEntityModel> Policies { get; set; }
        public DbSet<ClaimEntityModel> Claims { get; set; }
        public DbSet<PaymentEntityModel> Payments { get; set; }
        public DbSet<SupportTicketEntity> Tickets { get; set; }
    }
}
