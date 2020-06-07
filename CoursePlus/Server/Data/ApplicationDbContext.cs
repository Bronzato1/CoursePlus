using CoursePlus.Shared.Models;
using CoursePlus.Shared.Utilities;
using JsonNet.PrivateSettersContractResolvers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Syncfusion.Blazor.Notifications;
using System;
using System.IO;
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
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<WatchHistory> WatchHistories { get; set; }
        public DbSet<QuizTopic> QuizTopics { get; set; }
        public DbSet<QuizItem> QuizItems { get; set; }
        public DbSet<QuizProposal> QuizProposals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");
            builder.Seed();
            base.OnModelCreating(builder);

            builder.Entity<Enrollment>().HasKey(x => new { x.QuizTopicId, x.ProfileId });
            builder.Entity<WatchHistory>().HasKey(x => new { x.EpisodeId, x.ProfileId });
            builder.Entity<QuizTopic>().HasMany(x => x.Items).WithOne().HasForeignKey(x => x.QuizTopicId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<QuizTopic>().HasOne(x => x.Owner).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<QuizItem>().HasMany(x => x.Proposals).WithOne().HasForeignKey(x => x.QuizItemId).OnDelete(DeleteBehavior.Cascade);
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
        public  static void Initialize(ApplicationDbContext context)
        {
            // Appelé à chaque démarrage de l'application

            context.Database.EnsureCreated(); // <-- Provoque l'appel de la méthode Seed

            if (context.ChangeTracker.HasChanges())
                context.SaveChanges();
        }
        public  static void Seed(this ModelBuilder modelBuilder)
        {
            // Appelé à chaque démarrage de l'application à travers la méthode Initialize
            // Tout changement implique l'ajout d'une étape de migration

            SeedRoles(modelBuilder);
            SeedUsers(modelBuilder);
            SeedUserRoles(modelBuilder);
            SeedCategories(modelBuilder);
            //SeedBooks(modelBuilder);
            SeedProfiles(modelBuilder);
        }
        private static void SeedCategories(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData
            (
                new Category
                {
                    Id = 1,
                    Name = "Animaux"
                },
                new Category
                {
                    Id = 2,
                    Name = "Archéologie"
                },
                new Category
                {
                    Id = 3,
                    Name = "Musique"
                },
                new Category
                {
                    Id = 4,
                    Name = "Architecture"
                },
                new Category
                {
                    Id = 5,
                    Name = "Arts"
                },
                new Category
                {
                    Id = 6,
                    Name = "Bande dessinée"
                },
                new Category
                {
                    Id = 7,
                    Name = "Célébrités"
                },
                new Category
                {
                    Id = 8,
                    Name = "Cinéma"
                },
                new Category
                {
                    Id = 9,
                    Name = "Culture générale"
                },
                new Category
                {
                    Id = 10,
                    Name = "Gastronomie"
                },
                new Category
                {
                    Id = 11,
                    Name = "Géographie"
                },
                new Category
                {
                    Id = 12,
                    Name = "Histoire"
                },
                new Category
                {
                    Id = 13,
                    Name = "Informatique"
                },
                new Category
                {
                    Id = 14,
                    Name = "OpenBSD"
                },
                new Category
                {
                    Id = 15,
                    Name = "Littérature"
                },
                new Category
                {
                    Id = 16,
                    Name = "Loisirs"
                },
                new Category
                {
                    Id = 17,
                    Name = "Musique"
                },
                new Category
                {
                    Id = 18,
                    Name = "Nature"
                },
                new Category
                {
                    Id = 19,
                    Name = "Pays du monde"
                },
                new Category
                {
                    Id = 20,
                    Name = "Pour adultes"
                },
                new Category
                {
                    Id = 21,
                    Name = "Sciences"
                },
                new Category
                {
                    Id = 22,
                    Name = "Sports"
                },
                new Category
                {
                    Id = 23,
                    Name = "Télévision"
                },
                new Category
                {
                    Id = 24,
                    Name = "Tourisme"
                },
                new Category
                {
                    Id = 25,
                    Name = "Vie quotidienne"
                },
                new Category
                {
                    Id = 26,
                    Name = "Web"
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
                    PublishingDate = new DateTime(2019, 01, 01),
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
                },
                new Book
                {
                    Id = 5,
                    Title = "Illustrated C# 7",
                    Description = "<p> Get to work quickly with C# with a uniquely succinct and visual format used to present the C# 7.0 language. Whether you’re getting to grips with C# for the first time or working to deepen your understanding, you’ll find this book to be a clear and refreshing take on each aspect of the language.</p><p>Figures are of prime importance in this book. While teaching programming seminars,&nbsp;Daniel Solis found that he could almost watch the light bulbs going on over the students’ heads as he drew the figures on the whiteboard. In this text, he has distilled each important concept into simple but accurate illustrations. For this latest edition, Dan is joined by fellow experienced teacher and programmer,&nbsp;Cal Schrotenboer, to bring you the very latest C# language features, along with an understanding of the frameworks it most often lives in: .NET and the new .NET Core.</p><p>For something as intricate and precise as a programming language, there must be text as well as figures. But rather than long, wordy explanations, the authors use short, concise descriptions and bullet lists to make each important piece of information visually distinct and memorable.</p><p></p><p><b>What You’ll Learn</b><br></p><p></p><p></p><p></p><p></p><p></p><ul><li>Start with an overview of C# programming and how the language works under the hood<br></li><li>Put things in context with a little useful history of C# and .NET<br></li><li>Know how .NET Core fits into the picture<br></li><li>Understand how C# handles types</li><li>Benefit from clear, concise explanations of each language feature, from classes and inheritance to enumerators and iterators, and the new C# 7.0 tuples</li><li>Quickly access material via this book's visual introduction to asynchronous programming with C#</li></ul><p><b>Who This Book Is For</b></p><p>Novice to intermediate C# programmers, and more experienced programmers moving to C# from other languages</p>",
                    Author = "Daniel Solis",
                    Language = EnumLanguages.English,
                    CategoryId = 28,
                    PageCount = 799,
                    PublishingDate = new DateTime(2018, 01, 01),
                    PurchaseLink = "https://www.apress.com/gp/book/9781484232873",
                    ThumbnailUrl = "https://images.springer.com/sgw/books/medium/9781484232873.jpg",
                    ImageUrl = "https://images.springer.com/sgw/books/medium/9781484232873.jpg"
                },
                new Book
                {
                    Id = 6,
                    Title = "Interactive C#",
                    Description = "<p>Discover object - oriented programming with core concepts of C# in this unique tutorial. The book consists of four major sections which cover 15 core topics - nine of them are dedicated to object-oriented programming, five of them are dedicated to advanced concepts of C#, and one of them is dedicated to design patterns, with coverage of three Gang of Four design patterns with C# implementations. Finally,&nbsp;<i>Interactive C#</i>&nbsp;contains an FAQ section to cover all of these topics.</p><p>This book uniquely presents a two-way discussion&nbsp;between a teacher and students. So, with this book you will have the feel of learning C# in a classroom environment or with your private tutor. Your teacher will discuss the problems/topics and ask you questions; at the same time, counter questions are provided to clarify points where necessary.</p><p><b>What You Will Learn</b></p><ul><li>Become proficient in object-oriented programming<br></li><li>Remake yourself as a great C# programmer<br></li><li>Test your skills in C# fundamentals<br></li><li>Use Visual Studio to write, compile and execute your code</li></ul><b>Who This Book Is For</b><p>Programmers who want to understand the concepts and implementation of object-oriented programming in C#.</p>",
                    Author = "Vaskaran Sarcar",
                    Language = EnumLanguages.English,
                    CategoryId = 28,
                    PageCount = 494,
                    PublishingDate = new DateTime(2018, 01, 01),
                    PurchaseLink = "https://www.apress.com/gp/book/9781484233382",
                    ThumbnailUrl = "https://images.springer.com/sgw/books/medium/9781484233382.jpg",
                    ImageUrl = "https://images.springer.com/sgw/books/medium/9781484233382.jpg"
                },
                new Book
                {
                    Id = 7,
                    Title = "C# Programming for Absolute Beginners",
                    Description = "<p>Get started using the C# programming language. Based on the author’s 15 years of experience teaching beginners, the book provides you with a step-by-step introduction to the principles of programming, or rather, how to think like a programmer. The task-solution approach will get you immersed, with minimum theory and maximum action.<br><b>What You Will Learn</b></p><ul><li>Understand what programming is all about<br></li><li>Write simple, but non-trivial, programs<br></li><li>Become familiar with basic programming constructs such as statements, types, variables, conditions, and loops<br></li><li>Learn to think like a programmer and combine these programming constructs in new ways<br></li><li>Get to know C# as a modern, mainstream programming language, and Visual Studio as one of the world’s most popular programming tools</li></ul><b>Who This Book Is For</b><br>Those with very little or no experience in computer programming, who know how to use a computer, install a program, and navigate the web.",
                    Author = "Radek Vystavel",
                    Language = EnumLanguages.English,
                    CategoryId = 28,
                    PageCount = 356,
                    PublishingDate = new DateTime(2017, 01, 01),
                    PurchaseLink = "https://www.apress.com/gp/book/9781484233177",
                    ThumbnailUrl = "https://images.springer.com/sgw/books/medium/9781484233177.jpg",
                    ImageUrl = "https://images.springer.com/sgw/books/medium/9781484233177.jpg"
                },
                new Book
                {
                    Id = 8,
                    Title = "Pro C# 7",
                    Description = "<p>This essential classic title provides a comprehensive foundation in the C# programming language and the frameworks it lives in. Now in its 8th edition, you’ll find all the very latest C# 7.1 and .NET 4.7 features here, along with four brand new chapters on Microsoft’s lightweight, cross-platform framework, .NET Core, up to and including .NET Core 2.0. Coverage of ASP.NET Core, Entity Framework (EF) Core, and more, sits alongside the latest updates to .NET, including Windows Presentation Foundation (WPF), Windows Communication Foundation (WCF), and ASP.NET MVC.<br>Dive in and discover why <i>Pro C#</i> has been a favorite of C# developers worldwide for over 15 years. Gain a solid foundation in object-oriented development techniques, attributes and reflection, generics and collections as well as numerous advanced topics not found in other texts (such as CIL opcodes and emitting dynamic assemblies). With the help of this book you’ll have the confidence to put C# into practice and explore the .NET universe on your own terms.<br><b>What You Will Learn</b></p><ul><li>Discover the latest C# 7.1 features, from tuples to pattern matching<br></li><li>Hit the ground running with Microsoft’s lightweight, open source .NET Core platform, including ASP.NET Core MVC, ASP.NET Core web services, and Entity Framework Core<br></li><li>Find complete coverage of XAML, .NET 4.7, and Visual Studio 2017<br></li><li>Understand the philosophy behind .NET and the new, cross-platform alternative, .NET Core</li></ul>",
                    Author = "Andrew Troelsen",
                    Language = EnumLanguages.English,
                    CategoryId = 28,
                    PageCount = 1372,
                    PublishingDate = new DateTime(2017, 01, 01),
                    PurchaseLink = "https://www.apress.com/gp/book/9781484230176",
                    ThumbnailUrl = "https://images.springer.com/sgw/books/medium/9781484230176.jpg",
                    ImageUrl = "https://images.springer.com/sgw/books/medium/9781484230176.jpg"
                },
                new Book
                {
                    Id = 9,
                    Title = "Beginning XML with C# 7",
                    Description = "<p>Master the basics of XML as well as the namespaces and objects you need to know in order to work efficiently with XML.You’ll learn extensive support for XML in everything from data access to configuration, from raw parsing to code documentation.You will see clear, practical examples that illustrate best practices in implementing XML APIs and services as part of your C#-based Windows 10 applications.<br><i>Beginning XML with C# 7 </i>is completely revised to cover the XML features of .NET Framework 4.7 using C# 7 programming language. In this update, you’ll discover the tight integration of XML with ADO.NET and LINQ as well as additional .NET support for today’s RESTful web services and Web API.<br>Written by a Microsoft Most Valuable Professional and developer, this book demystifies everything to do with XML and C# 7.<br><b>What You Will Learn:</b></p><ul><li>Discover how XML works with the .NET Framework<br></li><li>Read, write, access, validate, and manipulate XML documents<br></li><li>Transform XML with XSLT<br></li><li>Use XML serialization and web services<br></li><li>Combine XML in ADO.NET and SQL Server<br></li><li>Create services using Windows Communication Foundation<br></li><li>Work with LINQ<br></li><li>Use XML with Web API and more<br></li></ul><b>Who This Book Is For :</b>Those with experience in C# and .NET new to the nuances of using XML.&nbsp; Some XML experience is helpful.",
                    Author = "Bipin Joshi",
                    Language = EnumLanguages.English,
                    CategoryId = 28,
                    PageCount = 453,
                    PublishingDate = new DateTime(2017, 01, 01),
                    PurchaseLink = "https://www.apress.com/gp/book/9781484231043",
                    ThumbnailUrl = "https://images.springer.com/sgw/books/medium/9781484231043.jpg",
                    ImageUrl = "https://images.springer.com/sgw/books/medium/9781484231043.jpg"
                },
                new Book
                {
                    Id = 10,
                    Title = "Selenium WebDriver Recipes in C#",
                    Description = "<p>Solve your Selenium WebDriver problems with this quick guide to automated testing of web applications with Selenium WebDriver in C#. <i>Selenium WebDriver Recipes in C#, Second Edition </i>contains hundreds of solutions to real-world problems, with clear explanations and ready - to - run Selenium test scripts that you can use in your own projects.</p> <p>You'll learn:</p> <ul> <li>How to locate web elements and test functions for hyperlinks, buttons, TextFields and TextAreas, radio buttons, CheckBoxes, and more</li> <li>How to use Selenium WebDriver for select lists, navigation, assertions, frames, file upload and pop-up dialogs</li> <li>How to debug test scripts and test data</li> <li>How to manage and deal with browser profiles and capabilities&lt;</li> <li>How to manage tests for advanced user interactions and experiences(UX)</li> <li>How to work with and manage tests and testing using Selenium Remote Control and Selenium Server</li></ul><br>AudienceThis book is for experienced.NET and C# Windows application programmers/developers.",
                    Author = "Zhimin Zhan",
                    Language = EnumLanguages.English,
                    CategoryId = 28,
                    PageCount = 164,
                    PublishingDate = new DateTime(2015, 01, 01),
                    PurchaseLink = "https://www.apress.com/gp/book/9781484217412",
                    ThumbnailUrl = "https://images.springer.com/sgw/books/medium/9781484217412.jpg",
                    ImageUrl = "https://images.springer.com/sgw/books/medium/9781484217412.jpg"
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
        private static void SeedUsers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData
            (
                new CustomUser
                {
                    Id = "e29790bd-b712-4594-8b3f-c13cbc2943ac",
                    UserName = Secret.AdminUserName.ToLower(),
                    NormalizedUserName = Secret.AdminUserName.ToUpper(),
                    Email = Secret.AdminEmail.ToLower(),
                    NormalizedEmail = Secret.AdminEmail.ToUpper(),
                    PasswordHash = Secret.AdminPasswordHash,
                    SecurityStamp = Secret.AdminSecurityStamp,
                    ConcurrencyStamp = Secret.AdminConcurrencyStamp,
                    LockoutEnabled = true,
                    FirstName = Secret.AdminFirstName,
                    LastName = Secret.AdminLastName,
                    AvatarId = null
                }
            ) ;
        }
        private static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "User", NormalizedName = "USER", Id = "0a3d93c9-e5d8-4ed5-b79d-d1e6a3768228", ConcurrencyStamp = "7ff46a67-2016-40cb-ab9d-3cbe2594018e" });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN", Id = "1002a5ed-a8e4-4c5c-9587-b8a8e1aa320b", ConcurrencyStamp = "9d0ed9a1-83a4-44b8-8de6-25e3d82dd1e9" });
        }
        private static void SeedUserRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> {UserId = "e29790bd-b712-4594-8b3f-c13cbc2943ac", RoleId = "0a3d93c9-e5d8-4ed5-b79d-d1e6a3768228" });
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { UserId = "e29790bd-b712-4594-8b3f-c13cbc2943ac", RoleId = "1002a5ed-a8e4-4c5c-9587-b8a8e1aa320b" });
        }
        private static void SeedProfiles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>().HasData
            (
                new Profile
                {
                    Id = 1,
                    UserId = "e29790bd-b712-4594-8b3f-c13cbc2943ac",
                    CreatedTime = new DateTime(2020, 05, 01),
                    UpdatedTime = new DateTime(2020, 05, 01),
                    CreatedUser = "azur.consult@gmail.com",
                    UpdatedUser = "azur.consult@gmail.com"
                }
            );
        }
    }
}