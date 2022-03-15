using CustomMembershipAuthServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomMembershipAuthServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AppUser> Users => Set<AppUser>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser { Id = 1, UserName = "bob", Email = "bob@test.com", Password = "1234", City = "New Jersey" },
                new AppUser { Id = 2, UserName = "alice", Email = "alice@test.com", Password = "1234", City = "London" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}