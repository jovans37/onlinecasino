using Microsoft.EntityFrameworkCore;
using OnlineCasino.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OnlineCasino.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public ApplicationDbContext()
        {
        }
        public DbSet<Bonus> Bonuses { get; set; }
        public DbSet<BonusAuditLog> BonusAuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Bonus configuration
            modelBuilder.Entity<Bonus>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasConversion<string>()  //used to store the value as string in db ex: "Welcome", "Deposit"
                    .HasMaxLength(20);

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(100);

                //add index on column for optimize the query
                entity.HasIndex(e => e.PlayerId);

                //unique constraint to prevent duplicate types for player
                entity.HasIndex(e => new { e.PlayerId, e.Type, e.IsActive })
                      .HasFilter("[IsActive] = 1")
                      .IsUnique();
            });

            // Audit log configuration
            modelBuilder.Entity<BonusAuditLog>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Action)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Operator)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasOne(e => e.Bonus)
                    .WithMany()
                    .HasForeignKey(e => e.BonusId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}