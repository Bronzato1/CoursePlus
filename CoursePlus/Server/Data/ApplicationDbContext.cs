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
        public DbSet<Image> Images { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }

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

            var email = _httpContextAccessor.HttpContext.User.Identity.Name;
            var firstName = _httpContextAccessor.HttpContext.User.FindFirstValue("FirstName");
            var lastName = _httpContextAccessor.HttpContext.User.FindFirstValue("LastName");
            var fullName = firstName + " " + lastName;
            var now = DateTime.UtcNow;

            foreach (var added in addedAuditedEntities)
            {
                added.CreatedTime = now;
                added.UpdatedTime = now;
                added.CreatedUser = email;
                added.UpdatedUser = email;
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                modified.UpdatedTime = now;
                modified.UpdatedUser = email;
            }

            return base.SaveChanges();
        }
    }

    public static class DbInitializer
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            SeedRoles(modelBuilder);
            SeedCategories(modelBuilder);
            SeedBooks(modelBuilder);
        }

        private static void SeedCategories(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData
            (
                new Category
                {
                    Id = 1,
                    Name = "HTML"
                },
                new Category
                {
                    Id = 2,
                    Name = "CSS"
                },
                new Category
                {
                    Id = 3,
                    Name = "JavaScript"
                },
                new Category
                {
                    Id = 4,
                    Name = "TypeScript"
                }
            );
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
                    Author = "John",
                    Language = EnumLanguages.English,
                    CategoryId = 1
                },
                new Book
                {
                    Id = 2,
                    Title = "Ccccc",
                    Description = "DDddd",
                    Author = "Cecilia",
                    Language = EnumLanguages.English,
                    CategoryId = 2
                },
                new Book
                {
                    Id = 3,
                    Title = "Eeeee",
                    Description = "Fffff",
                    Author = "Mike",
                    Language = EnumLanguages.French,
                    CategoryId = 3
                },
                new Book
                {
                    Id = 4,
                    Title = "Gggggg",
                    Description = "Hhhhhh",
                    Author = "Steve",
                    Language = EnumLanguages.English,
                    CategoryId = 4
                }
            );
        }

        private static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "User", NormalizedName = "USER", Id = "0a3d93c9-e5d8-4ed5-b79d-d1e6a3768228", ConcurrencyStamp = "7ff46a67-2016-40cb-ab9d-3cbe2594018e" });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN", Id = "1002a5ed-a8e4-4c5c-9587-b8a8e1aa320b", ConcurrencyStamp = "9d0ed9a1-83a4-44b8-8de6-25e3d82dd1e9" });
        }

        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.ChangeTracker.HasChanges())
                context.SaveChanges();
        }

    }
}