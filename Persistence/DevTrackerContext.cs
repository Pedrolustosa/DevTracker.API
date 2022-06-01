using DevTracker.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevTracker.API.Persistence
{
    public class DevTrackerContext : DbContext
    {
        public DevTrackerContext(DbContextOptions<DevTrackerContext> options)
        : base(options)
        { }

        public DbSet<Package>? Packages { get; set; }
        public DbSet<PackageUpdate>? PackageUpdates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Package>(e =>
            {
                //e.ToTable("tb_Package");

                e.HasKey(p => p.Id);
                e.HasMany(p => p.Updates)
                 .WithOne()
                 .HasForeignKey(pu => pu.PackageId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<PackageUpdate>(e =>
            {
                e.HasKey(p => p.Id);
            });
        }
    }
}