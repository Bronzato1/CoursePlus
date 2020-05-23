using CoursePlus.Shared.Models;
using CoursePlus.Shared.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
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
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<WatchHistory> WatchHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");
            builder.Seed();
            base.OnModelCreating(builder);

            builder.Entity<Enrollment>().HasKey(c => new { c.PlaylistId, c.ProfileId });
            builder.Entity<WatchHistory>().HasKey(c => new { c.EpisodeId, c.ProfileId });
            builder.Entity<Playlist>().HasOne(x => x.Profile).WithMany().OnDelete(DeleteBehavior.NoAction);
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
            // Appelé à chaque démarrage de l'application à travers la méthode Initialize
            // Tout changement implique l'ajout d'une étape de migration

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
                    Name = "Film & Animation"
                },
                new Category
                {
                    Id = 2,
                    Name = "Autos & Vehicles"
                },
                new Category
                {
                    Id = 10,
                    Name = "Music"
                },
                new Category
                {
                    Id = 15,
                    Name = "Pets & Animals"
                },
                new Category
                {
                    Id = 17,
                    Name = "Sports"
                },
                new Category
                {
                    Id = 19,
                    Name = "Travel & Events"
                },
                new Category
                {
                    Id = 20,
                    Name = "Gaming"
                },
                new Category
                {
                    Id = 22,
                    Name = "People & Blogs"
                },
                new Category
                {
                    Id = 23,
                    Name = "Comedy"
                },
                new Category
                {
                    Id = 24,
                    Name = "Entertainment"
                },
                new Category
                {
                    Id = 25,
                    Name = "News & Politics"
                },
                new Category
                {
                    Id = 26,
                    Name = "Howto & Style"
                },
                new Category
                {
                    Id = 27,
                    Name = "Education"
                },
                new Category
                {
                    Id = 28,
                    Name = "Science & Technology"
                },
                new Category
                {
                    Id = 29,
                    Name = "Nonprofits & Activism"
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
                    Title = "Developing 2D Games with Unity",
                    Description = "<p>Follow a walkthrough of the Unity Engine and learn important 2D-centric lessons in scripting, working with image assets, animations, cameras, collision detection, and state management. In addition to the fundamentals, you'll learn best practices, helpful game-architectural patterns, and how to customize Unity to suit your needs, all in the context of building a working 2D game.<br> </p><p>While many books focus on 3D game creation with Unity, the easiest market for an independent developer to thrive in is 2D games. 2D games are generally cheaper to produce, more feasible for small teams, and more likely to be completed. If you live and breathe games and want to create them then 2D games are a great place to start.&nbsp;</p><p>By focusing exclusively on 2D games and Unity’s ever-expanding 2D workflow, this book gives aspiring independent game developers the tools they need to thrive. Various real-world examples of independent games are used to teach fundamental concepts of developing 2D games in Unity, using the very latest tools in Unity’s updated 2D workflow.&nbsp;</p><p> New all-digital channels for distribution, such as Nintendo eShop, XBox Live Marketplace, the Playstation Store, the App Store, Google Play, itch.io, Steam, and GOG.com have made it easier than ever to discover, buy, and sell games. The golden age of independent gaming is upon us, and there has never been a better time to get creative, roll up your sleeves, and build that game you’ve always dreamed about. <i>Developing 2D Games with Unity</i> can show you the way.</p><p><b>What You'll Learn</b><br></p><p></p><ul><li>Delve deeply into useful 2D topics, such as sprites, tile slicing, and the brand new Tilemap feature.<br></li><li>Build a working 2D RPG-style game as you learn.</li><li>Construct a flexible and extensible game architecture using Unity-specific tools like Scriptable Objects, Cinemachine, and Prefabs.</li><li>Take advantage of the streamlined 2D workflow provided by the Unity environment.</li><li>&nbsp;Deploy games to desktop</li></ul><p></p><p></p><p><b>Who This Book Is For</b></p><p><b> </b></p><p>Hobbyists with some knowledge of programming, as well as seasoned programmers interested in learning to make games independent of a major studio.<br></p>",
                    Author = "Jared Halpern",
                    Language = EnumLanguages.English,
                    CategoryId = 28,
                    PageCount = 383,
                    PublishingDate = new DateTime(2019,01,01),
                    PurchaseLink = "https://www.apress.com/gp/book/9781484237717",
                    ThumbnailUrl = "https://images.springer.com/sgw/books/medium/9781484237717.jpg",  
                    ImageUrl = "https://images.springer.com/sgw/books/medium/9781484237717.jpg"
                },
                new Book
                {
                    Id = 2,
                    Title = "Beginning iOS AR Game Development",
                    Description = "<p>Create a fully featured application that’s both sophisticated and engaging. This book provides a detailed guide in developing augmented reality games that can take advantage of the advanced capabilities of new iOS devices and code while also offering compatibility with still supported legacy devices.&nbsp;</p><p>No programming experience is necessary as this book begins on the ground floor with basic programming concepts in Unity and builds to incorporating input from the real world to create interactive realities. You’ll learn to program with the Unity 2017 development platform using C#.&nbsp;</p><p>Recent announcements of increased AR capabilities on the latest iPhones and iPads show a clear dedication on Apple’s part to this emerging market of immersive games and apps. Unity 2017 is the latest version of this industry leading development platform and C# is a ubiquitous programming language perfect for any programmer to begin with.&nbsp;</p><p>Using the latest development technologies, <i>Beginning iOS AR Game Development</i> will show you how to program games that interact directly with the real world environment around the user for creative fantastic augmented reality experiences.</p><p><b>What You'll Learn</b><br></p><p></p><ul><li>Download assets from the Unity store<br></li><li>Create a scene in Unity 2017<br></li><li>Use physics and controls on mobile devices<br></li></ul><p></p><b>Who This Book Is For</b><b><br></b>Beginner programmers and/or people new to developing games using Unity. It also serves as a great introduction to developing AR games and educators teaching the subject at high school or higher levels.",
                    Author = "Allan Fowler",
                    Language = EnumLanguages.English,
                    CategoryId = 28,
                    PageCount = 244,
                    PublishingDate = new DateTime(2019, 01, 01),
                    PurchaseLink = "https://www.apress.com/gp/book/9781484236178",
                    ThumbnailUrl = "https://images.springer.com/sgw/books/medium/9781484236178.jpg",
                    ImageUrl = "https://images.springer.com/sgw/books/medium/9781484236178.jpg"
                },
                new Book
                {
                    Id = 3,
                    Title = "Design Patterns in C#",
                    Description = "<p>Get hands-on experience with each Gang of Four design pattern using C#. For each of the patterns, you’ll see at least one real-world scenario, a coding example, and a complete implementation including output.<br>In the first part of <i>Design Patterns in C#</i>, you will cover the 23 Gang of Four (GoF) design patterns, before moving onto some alternative design patterns, including the Simple Factory Pattern, the Null Object Pattern, and the MVC Pattern. The final part winds up with a conclusion and criticisms of design patterns with chapters on anti-patterns and memory leaks. By working through easy-to-follow examples, you will understand the concepts in depth and have a collection of programs to port over to your own projects.<br>Along the way, the author discusses the different creational, structural, and behavioral patterns and why such classifications are useful.&nbsp;In each of these chapters, there is a Q&amp;A session that clears up any doubts and covers the pros and cons of each of these patterns.He finishes the book with FAQs that will help you consolidate your knowledge. This book presents the topic of design patterns in C# in such a way that anyone can grasp the idea.&nbsp;<b><br></b><b>What You Will Learn</b></p><ul><li>Work with each of the design patterns<br></li><li>Implement the design patterns in real-world applications<br></li><li>Select an alternative to these patterns by comparing their pros and cons<br></li><li>Use Visual Studio Community Edition 2017 to write code and generate output<br></li></ul><b>Who This Book Is For</b><br><b><br></b>Software developers, software testers, and software architects.",
                    Author = "Vaskaran Sarcar",
                    Language = EnumLanguages.English,
                    CategoryId = 28,
                    PageCount = 455,
                    PublishingDate = new DateTime(2018, 01, 01),
                    PurchaseLink = "https://www.apress.com/gp/book/9781484236390",
                    ThumbnailUrl = "https://images.springer.com/sgw/books/medium/9781484236390.jpg",
                    ImageUrl = "https://images.springer.com/sgw/books/medium/9781484236390.jpg"
                },
                new Book
                {
                    Id = 4,
                    Title = "Design Patterns in Modern C++",
                    Description = "<p>Apply modern C++17 to the implementations of classic design patterns. As well as covering traditional design patterns, this book fleshes out new patterns and approaches that will be useful to C++ developers. The author presents concepts as a fun investigation of how problems can be solved in different ways, along the way using varying degrees of technical sophistication and explaining different sorts of trade-offs.<br><i>Design Patterns in Modern C++ </i>also provides a technology demo for modern C++, showcasing how some of its latest features (e.g., coroutines) make difficult problems a lot easier to solve. The examples in this book are all suitable for putting into production, with only a few simplifications made in order to aid readability.<br><b>What You Will Learn</b><br></p><ul><li>Apply design patterns to modern C++ programming<br></li><li>Use creational patterns of builder, factories, prototype and singleton<br></li><li>Implement structural patterns such as adapter, bridge, decorator, facade and more<br></li><li>Work with the behavioral patterns such as chain of responsibility, command, iterator, mediator and more<br></li><li>Apply functional design patterns such as Monad and more<br></li></ul><b><br></b><b>Who This Book Is For</b><br>Those with at least some prior programming experience, especially in C++.",
                    Author = "Dmitri Nesteruk",
                    Language = EnumLanguages.English,
                    CategoryId = 28,
                    PageCount = 314,
                    PublishingDate = new DateTime(2018, 01, 01),
                    PurchaseLink = "https://www.apress.com/gp/book/9781484236024",
                    ThumbnailUrl = "https://images.springer.com/sgw/books/medium/9781484236024.jpg",
                    ImageUrl = "https://images.springer.com/sgw/books/medium/9781484236024.jpg"
                }
                //new Book
                //{
                //    Id = 5,
                //    Title = "",
                //    Description = "",
                //    Author = "",
                //    Language = EnumLanguages.English,
                //    CategoryId = 28,
                //    PageCount = 0,
                //    PublishingDate = new DateTime(2019, 01, 01),
                //    PurchaseLink = "",
                //    ThumbnailUrl = "",
                //    ImageUrl = ""
                //}
            );
        }

        private static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "User", NormalizedName = "USER", Id = "0a3d93c9-e5d8-4ed5-b79d-d1e6a3768228", ConcurrencyStamp = "7ff46a67-2016-40cb-ab9d-3cbe2594018e" });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN", Id = "1002a5ed-a8e4-4c5c-9587-b8a8e1aa320b", ConcurrencyStamp = "9d0ed9a1-83a4-44b8-8de6-25e3d82dd1e9" });
        }

        public static void Initialize(ApplicationDbContext context)
        {
            // Appelé à chaque démarrage de l'application

            context.Database.EnsureCreated(); // <-- Provoque l'appel de la méthode Seed

            if (context.ChangeTracker.HasChanges())
                context.SaveChanges();
        }

    }
}