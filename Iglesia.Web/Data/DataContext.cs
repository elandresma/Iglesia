using Iglesia.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iglesia.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Region> Regions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Region>()
                .HasIndex(t => t.Name)
                .IsUnique();
        }
    }
}

