using DevPro.Domain.Entities;
using DevPro.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevPro.Database
{
    public class DevProLocalDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DevProLocalDbContext(DbContextOptions<DevProLocalDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call base first for Identity configurations
            base.OnModelCreating(modelBuilder);

            // Your existing configurations
            modelBuilder.Entity<Product>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<Product>()
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("NOW()");

            // Optional: Customize Identity tables if needed
            modelBuilder.Entity<ApplicationUser>(b =>
            {
                // Customize user table
                b.ToTable("Users");
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("Roles");
            });
        }

        public DbSet<Product> Products { get; set; }

        // Identity will automatically add these DbSets:
        // Users, Roles, UserClaims, UserLogins, UserRoles, RoleClaims, UserTokens
    }
}