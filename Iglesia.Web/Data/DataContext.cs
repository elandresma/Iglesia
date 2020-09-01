using Iglesia.Common.Entities;
using Iglesia.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Iglesia.Web.Data
{
    public class DataContext :  IdentityDbContext<User>

    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Church> Churches { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Profession> Professions { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Region>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<District>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<Church>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<Profession>()
                .HasIndex(t => t.Name)
                .IsUnique();
        }
    }
}

