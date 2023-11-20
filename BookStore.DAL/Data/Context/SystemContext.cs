using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DAL
{
    public class SystemContext : IdentityDbContext
    {
        public DbSet<Book> Books => Set<Book>();
        public SystemContext(DbContextOptions<SystemContext> options)
        : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var passwordHasher = new PasswordHasher<IdentityUser>();
            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "1",
                    UserName = "Admin",
                    PasswordHash = passwordHasher.HashPassword(null,"AdminPassword123")
                },
                new IdentityUser
                {
                    Id = "2",
                    UserName = "Yasmeen",
                    PasswordHash = passwordHasher.HashPassword(null, "AdminPassword123")
                },
                new IdentityUser
                {
                    Id = "3",
                    UserName = "Khaled",
                    PasswordHash = passwordHasher.HashPassword(null, "AdminPassword123")
                }
            );


            var adminClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "admin")
            };

            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(
                adminClaims.Select((claim, index) => new IdentityUserClaim<string>
                {
                    Id = index + 1,
                    UserId = "1",
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                }).ToArray()
            );
        }
    }
}
