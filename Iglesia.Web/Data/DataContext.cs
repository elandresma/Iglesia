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

            modelBuilder.Entity<Region>(re =>
            {
                re.HasIndex("Name").IsUnique();
                re.HasMany(r => r.Districts).WithOne(d => d.Region).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<District>(di =>
            {
                di.HasIndex("Name", "RegionId").IsUnique();
                di.HasOne(d => d.Region).WithMany(c => c.Districts).OnDelete(DeleteBehavior.Cascade);
            });
          
            modelBuilder.Entity<Church>(ch =>
            {
                ch.HasIndex("Name", "DistrictId").IsUnique();
                ch.HasOne(c => c.District).WithMany(d => d.Churches).OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Profession>()
                .HasIndex(t => t.Name)
                .IsUnique();
        }
    }
}

