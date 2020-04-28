using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using static CoursePlus.Server.Data.ApplicationDbContext;

namespace CoursePlus.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<CustomUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");
            builder.Seed();
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            var addedAuditedEntities = ChangeTracker
                .Entries<IAuditable>()
                .Where(p => p.State == EntityState.Added)
                .Select(p => p.Entity);

            var modifiedAuditedEntities = ChangeTracker
                .Entries<IAuditable>()
                .Where(p => p.State == EntityState.Modified)
                .Select(p => p.Entity);

            // var email = _httpContextAccessor.HttpContext.User.Identity.Name;
            var firstName = _httpContextAccessor.HttpContext.User.FindFirstValue("FirstName");
            var lastName = _httpContextAccessor.HttpContext.User.FindFirstValue("LastName");
            var fullName = firstName + " " + lastName;
            var now = DateTime.UtcNow;

            foreach (var added in addedAuditedEntities)
            {
                added.CreatedTime = now;
                added.UpdatedTime = now;
                added.CreatedUser = fullName;
                added.UpdatedUser = fullName;
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                modified.UpdatedTime = now;
                modified.UpdatedUser = fullName;
            }

            return base.SaveChanges();
        }
    }

    public class CustomUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
    }

    public static class DbInitializer
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            SeedRoles(modelBuilder);
            SeedBooks(modelBuilder);
        }

        private static void SeedBooks(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData
            (
                new Book
                {
                    Id = 1,
                    Title = "Aaaaa",
                    Description = "Bbbbbb",
                    Author = "John"
                },
                new Book
                {
                    Id = 2,
                    Title = "Ccccc",
                    Description = "DDddd",
                    Author = "Cecilia"
                },
                new Book
                {
                    Id = 3,
                    Title = "Eeeee",
                    Description = "Fffff",
                    Author = "Mike"
                },
                new Book
                {
                    Id = 4,
                    Title = "Gggggg",
                    Description = "Hhhhhh",
                    Author = "Steve"
                }
            );
        }

        private static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "User", NormalizedName = "USER", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() });
        }

        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.ChangeTracker.HasChanges())
                context.SaveChanges();
        }

    }
}