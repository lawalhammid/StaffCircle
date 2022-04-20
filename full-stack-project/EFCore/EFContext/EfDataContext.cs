using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace EFCore.EFContext
{
    public class EfDataContext : DbContext
    {
        public EfDataContext(DbContextOptions<EfDataContext> options) : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<SendMessages> SendMessages { get; set; }
        public DbSet<ComposedMessages> ComposedMessages { get; set; }
        public DbSet<AuditTrail> AuditTrail { get; set; }
        public DbSet<Country> Country { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    CountryName = "UK",
                    PhonePrefix= "+44"
                }
            );

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // modelBuilder.Entity<Users>().HasKey(ma => new { ma.Id });
        }

    }
}
