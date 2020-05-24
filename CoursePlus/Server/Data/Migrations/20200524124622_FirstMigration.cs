using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlus.Server.Data.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Avatars",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Thumbnails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thumbnails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    AvatarId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Avatars_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "Avatars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    PurchaseLink = table.Column<string>(nullable: true),
                    PageCount = table.Column<int>(nullable: false),
                    Language = table.Column<int>(nullable: false),
                    PublishingDate = table.Column<DateTime>(nullable: false),
                    Featured = table.Column<bool>(nullable: false),
                    Popular = table.Column<bool>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    ImageId = table.Column<int>(nullable: true),
                    ThumbnailUrl = table.Column<string>(nullable: true),
                    ThumbnailId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(nullable: true),
                    UpdatedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Books_Thumbnails_ThumbnailId",
                        column: x => x.ThumbnailId,
                        principalTable: "Thumbnails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Joined = table.Column<DateTime>(nullable: false),
                    Enrolled = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(nullable: true),
                    UpdatedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    SubTitle = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    Language = table.Column<int>(nullable: false),
                    Difficulty = table.Column<int>(nullable: false),
                    ImageId = table.Column<int>(nullable: true),
                    OwnerId = table.Column<int>(nullable: false),
                    Featured = table.Column<bool>(nullable: false),
                    Popular = table.Column<bool>(nullable: false),
                    ThumbnailId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(nullable: true),
                    UpdatedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Playlists_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Playlists_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Playlists_Profiles_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Playlists_Thumbnails_ThumbnailId",
                        column: x => x.ThumbnailId,
                        principalTable: "Thumbnails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    PlaylistId = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(nullable: true),
                    UpdatedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapters_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    PlaylistId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => new { x.PlaylistId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_Enrollments_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Duration = table.Column<int>(nullable: false),
                    Trailer = table.Column<string>(nullable: true),
                    ChapterId = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(nullable: true),
                    UpdatedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episodes_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WatchHistories",
                columns: table => new
                {
                    EpisodeId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(nullable: true),
                    UpdatedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchHistories", x => new { x.EpisodeId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_WatchHistories_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchHistories_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0a3d93c9-e5d8-4ed5-b79d-d1e6a3768228", "7ff46a67-2016-40cb-ab9d-3cbe2594018e", "User", "USER" },
                    { "1002a5ed-a8e4-4c5c-9587-b8a8e1aa320b", "9d0ed9a1-83a4-44b8-8de6-25e3d82dd1e9", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AvatarId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "e29790bd-b712-4594-8b3f-c13cbc2943ac", 0, null, "803efe9a-0e73-4bdc-b7cb-bc6a61438662", "azur.consult@gmail.com", false, "John", "Doe", true, null, "AZUR.CONSULT@GMAIL.COM", "AZUR.CONSULT@GMAIL.COM", "AQAAAAEAACcQAAAAEF1U8tBWJdEBcTyOsj0imq1JhvieXmbNRHMr8TKmjWlYVy+xc80JWOXEfrS/tLdNSw==", null, false, "IDTUR5JQA5ZLV2MDXIB44ZBBPPJKUWMS", false, "azur.consult@gmail.com" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 19, "Travel & Events" },
                    { 27, "Education" },
                    { 28, "Science & Technology" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { "e29790bd-b712-4594-8b3f-c13cbc2943ac", "0a3d93c9-e5d8-4ed5-b79d-d1e6a3768228" },
                    { "e29790bd-b712-4594-8b3f-c13cbc2943ac", "1002a5ed-a8e4-4c5c-9587-b8a8e1aa320b" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "CreatedTime", "CreatedUser", "Description", "Featured", "ImageId", "ImageUrl", "Language", "PageCount", "Popular", "PublishingDate", "PurchaseLink", "ThumbnailId", "ThumbnailUrl", "Title", "UpdatedTime", "UpdatedUser" },
                values: new object[,]
                {
                    { 1, "Jared Halpern", 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "<p>Follow a walkthrough of the Unity Engine and learn important 2D-centric lessons in scripting, working with image assets, animations, cameras, collision detection, and state management. In addition to the fundamentals, you'll learn best practices, helpful game-architectural patterns, and how to customize Unity to suit your needs, all in the context of building a working 2D game.<br> </p><p>While many books focus on 3D game creation with Unity, the easiest market for an independent developer to thrive in is 2D games. 2D games are generally cheaper to produce, more feasible for small teams, and more likely to be completed. If you live and breathe games and want to create them then 2D games are a great place to start.&nbsp;</p><p>By focusing exclusively on 2D games and Unity’s ever-expanding 2D workflow, this book gives aspiring independent game developers the tools they need to thrive. Various real-world examples of independent games are used to teach fundamental concepts of developing 2D games in Unity, using the very latest tools in Unity’s updated 2D workflow.&nbsp;</p><p> New all-digital channels for distribution, such as Nintendo eShop, XBox Live Marketplace, the Playstation Store, the App Store, Google Play, itch.io, Steam, and GOG.com have made it easier than ever to discover, buy, and sell games. The golden age of independent gaming is upon us, and there has never been a better time to get creative, roll up your sleeves, and build that game you’ve always dreamed about. <i>Developing 2D Games with Unity</i> can show you the way.</p><p><b>What You'll Learn</b><br></p><p></p><ul><li>Delve deeply into useful 2D topics, such as sprites, tile slicing, and the brand new Tilemap feature.<br></li><li>Build a working 2D RPG-style game as you learn.</li><li>Construct a flexible and extensible game architecture using Unity-specific tools like Scriptable Objects, Cinemachine, and Prefabs.</li><li>Take advantage of the streamlined 2D workflow provided by the Unity environment.</li><li>&nbsp;Deploy games to desktop</li></ul><p></p><p></p><p><b>Who This Book Is For</b></p><p><b> </b></p><p>Hobbyists with some knowledge of programming, as well as seasoned programmers interested in learning to make games independent of a major studio.<br></p>", false, null, "https://images.springer.com/sgw/books/medium/9781484237717.jpg", 0, 383, false, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://www.apress.com/gp/book/9781484237717", null, "https://images.springer.com/sgw/books/medium/9781484237717.jpg", "Developing 2D Games with Unity", null, null },
                    { 2, "Allan Fowler", 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "<p>Create a fully featured application that’s both sophisticated and engaging. This book provides a detailed guide in developing augmented reality games that can take advantage of the advanced capabilities of new iOS devices and code while also offering compatibility with still supported legacy devices.&nbsp;</p><p>No programming experience is necessary as this book begins on the ground floor with basic programming concepts in Unity and builds to incorporating input from the real world to create interactive realities. You’ll learn to program with the Unity 2017 development platform using C#.&nbsp;</p><p>Recent announcements of increased AR capabilities on the latest iPhones and iPads show a clear dedication on Apple’s part to this emerging market of immersive games and apps. Unity 2017 is the latest version of this industry leading development platform and C# is a ubiquitous programming language perfect for any programmer to begin with.&nbsp;</p><p>Using the latest development technologies, <i>Beginning iOS AR Game Development</i> will show you how to program games that interact directly with the real world environment around the user for creative fantastic augmented reality experiences.</p><p><b>What You'll Learn</b><br></p><p></p><ul><li>Download assets from the Unity store<br></li><li>Create a scene in Unity 2017<br></li><li>Use physics and controls on mobile devices<br></li></ul><p></p><b>Who This Book Is For</b><b><br></b>Beginner programmers and/or people new to developing games using Unity. It also serves as a great introduction to developing AR games and educators teaching the subject at high school or higher levels.", false, null, "https://images.springer.com/sgw/books/medium/9781484236178.jpg", 0, 244, false, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://www.apress.com/gp/book/9781484236178", null, "https://images.springer.com/sgw/books/medium/9781484236178.jpg", "Beginning iOS AR Game Development", null, null },
                    { 3, "Vaskaran Sarcar", 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "<p>Get hands-on experience with each Gang of Four design pattern using C#. For each of the patterns, you’ll see at least one real-world scenario, a coding example, and a complete implementation including output.<br>In the first part of <i>Design Patterns in C#</i>, you will cover the 23 Gang of Four (GoF) design patterns, before moving onto some alternative design patterns, including the Simple Factory Pattern, the Null Object Pattern, and the MVC Pattern. The final part winds up with a conclusion and criticisms of design patterns with chapters on anti-patterns and memory leaks. By working through easy-to-follow examples, you will understand the concepts in depth and have a collection of programs to port over to your own projects.<br>Along the way, the author discusses the different creational, structural, and behavioral patterns and why such classifications are useful.&nbsp;In each of these chapters, there is a Q&amp;A session that clears up any doubts and covers the pros and cons of each of these patterns.He finishes the book with FAQs that will help you consolidate your knowledge. This book presents the topic of design patterns in C# in such a way that anyone can grasp the idea.&nbsp;<b><br></b><b>What You Will Learn</b></p><ul><li>Work with each of the design patterns<br></li><li>Implement the design patterns in real-world applications<br></li><li>Select an alternative to these patterns by comparing their pros and cons<br></li><li>Use Visual Studio Community Edition 2017 to write code and generate output<br></li></ul><b>Who This Book Is For</b><br><b><br></b>Software developers, software testers, and software architects.", false, null, "https://images.springer.com/sgw/books/medium/9781484236390.jpg", 0, 455, false, new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://www.apress.com/gp/book/9781484236390", null, "https://images.springer.com/sgw/books/medium/9781484236390.jpg", "Design Patterns in C#", null, null },
                    { 4, "Dmitri Nesteruk", 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "<p>Apply modern C++17 to the implementations of classic design patterns. As well as covering traditional design patterns, this book fleshes out new patterns and approaches that will be useful to C++ developers. The author presents concepts as a fun investigation of how problems can be solved in different ways, along the way using varying degrees of technical sophistication and explaining different sorts of trade-offs.<br><i>Design Patterns in Modern C++ </i>also provides a technology demo for modern C++, showcasing how some of its latest features (e.g., coroutines) make difficult problems a lot easier to solve. The examples in this book are all suitable for putting into production, with only a few simplifications made in order to aid readability.<br><b>What You Will Learn</b><br></p><ul><li>Apply design patterns to modern C++ programming<br></li><li>Use creational patterns of builder, factories, prototype and singleton<br></li><li>Implement structural patterns such as adapter, bridge, decorator, facade and more<br></li><li>Work with the behavioral patterns such as chain of responsibility, command, iterator, mediator and more<br></li><li>Apply functional design patterns such as Monad and more<br></li></ul><b><br></b><b>Who This Book Is For</b><br>Those with at least some prior programming experience, especially in C++.", false, null, "https://images.springer.com/sgw/books/medium/9781484236024.jpg", 0, 314, false, new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://www.apress.com/gp/book/9781484236024", null, "https://images.springer.com/sgw/books/medium/9781484236024.jpg", "Design Patterns in Modern C++", null, null },
                    { 5, "Daniel Solis", 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "<p> Get to work quickly with C# with a uniquely succinct and visual format used to present the C# 7.0 language. Whether you’re getting to grips with C# for the first time or working to deepen your understanding, you’ll find this book to be a clear and refreshing take on each aspect of the language.</p><p>Figures are of prime importance in this book. While teaching programming seminars,&nbsp;Daniel Solis found that he could almost watch the light bulbs going on over the students’ heads as he drew the figures on the whiteboard. In this text, he has distilled each important concept into simple but accurate illustrations. For this latest edition, Dan is joined by fellow experienced teacher and programmer,&nbsp;Cal Schrotenboer, to bring you the very latest C# language features, along with an understanding of the frameworks it most often lives in: .NET and the new .NET Core.</p><p>For something as intricate and precise as a programming language, there must be text as well as figures. But rather than long, wordy explanations, the authors use short, concise descriptions and bullet lists to make each important piece of information visually distinct and memorable.</p><p></p><p><b>What You’ll Learn</b><br></p><p></p><p></p><p></p><p></p><p></p><ul><li>Start with an overview of C# programming and how the language works under the hood<br></li><li>Put things in context with a little useful history of C# and .NET<br></li><li>Know how .NET Core fits into the picture<br></li><li>Understand how C# handles types</li><li>Benefit from clear, concise explanations of each language feature, from classes and inheritance to enumerators and iterators, and the new C# 7.0 tuples</li><li>Quickly access material via this book's visual introduction to asynchronous programming with C#</li></ul><p><b>Who This Book Is For</b></p><p>Novice to intermediate C# programmers, and more experienced programmers moving to C# from other languages</p>", false, null, "https://images.springer.com/sgw/books/medium/9781484232873.jpg", 0, 799, false, new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://www.apress.com/gp/book/9781484232873", null, "https://images.springer.com/sgw/books/medium/9781484232873.jpg", "Illustrated C# 7", null, null },
                    { 6, "Vaskaran Sarcar", 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "<p>Discover object - oriented programming with core concepts of C# in this unique tutorial. The book consists of four major sections which cover 15 core topics - nine of them are dedicated to object-oriented programming, five of them are dedicated to advanced concepts of C#, and one of them is dedicated to design patterns, with coverage of three Gang of Four design patterns with C# implementations. Finally,&nbsp;<i>Interactive C#</i>&nbsp;contains an FAQ section to cover all of these topics.</p><p>This book uniquely presents a two-way discussion&nbsp;between a teacher and students. So, with this book you will have the feel of learning C# in a classroom environment or with your private tutor. Your teacher will discuss the problems/topics and ask you questions; at the same time, counter questions are provided to clarify points where necessary.</p><p><b>What You Will Learn</b></p><ul><li>Become proficient in object-oriented programming<br></li><li>Remake yourself as a great C# programmer<br></li><li>Test your skills in C# fundamentals<br></li><li>Use Visual Studio to write, compile and execute your code</li></ul><b>Who This Book Is For</b><p>Programmers who want to understand the concepts and implementation of object-oriented programming in C#.</p>", false, null, "https://images.springer.com/sgw/books/medium/9781484233382.jpg", 0, 494, false, new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://www.apress.com/gp/book/9781484233382", null, "https://images.springer.com/sgw/books/medium/9781484233382.jpg", "Interactive C#", null, null },
                    { 7, "Radek Vystavel", 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "<p>Get started using the C# programming language. Based on the author’s 15 years of experience teaching beginners, the book provides you with a step-by-step introduction to the principles of programming, or rather, how to think like a programmer. The task-solution approach will get you immersed, with minimum theory and maximum action.<br><b>What You Will Learn</b></p><ul><li>Understand what programming is all about<br></li><li>Write simple, but non-trivial, programs<br></li><li>Become familiar with basic programming constructs such as statements, types, variables, conditions, and loops<br></li><li>Learn to think like a programmer and combine these programming constructs in new ways<br></li><li>Get to know C# as a modern, mainstream programming language, and Visual Studio as one of the world’s most popular programming tools</li></ul><b>Who This Book Is For</b><br>Those with very little or no experience in computer programming, who know how to use a computer, install a program, and navigate the web.", false, null, "https://images.springer.com/sgw/books/medium/9781484233177.jpg", 0, 356, false, new DateTime(2017, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://www.apress.com/gp/book/9781484233177", null, "https://images.springer.com/sgw/books/medium/9781484233177.jpg", "C# Programming for Absolute Beginners", null, null },
                    { 8, "Andrew Troelsen", 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "<p>This essential classic title provides a comprehensive foundation in the C# programming language and the frameworks it lives in. Now in its 8th edition, you’ll find all the very latest C# 7.1 and .NET 4.7 features here, along with four brand new chapters on Microsoft’s lightweight, cross-platform framework, .NET Core, up to and including .NET Core 2.0. Coverage of ASP.NET Core, Entity Framework (EF) Core, and more, sits alongside the latest updates to .NET, including Windows Presentation Foundation (WPF), Windows Communication Foundation (WCF), and ASP.NET MVC.<br>Dive in and discover why <i>Pro C#</i> has been a favorite of C# developers worldwide for over 15 years. Gain a solid foundation in object-oriented development techniques, attributes and reflection, generics and collections as well as numerous advanced topics not found in other texts (such as CIL opcodes and emitting dynamic assemblies). With the help of this book you’ll have the confidence to put C# into practice and explore the .NET universe on your own terms.<br><b>What You Will Learn</b></p><ul><li>Discover the latest C# 7.1 features, from tuples to pattern matching<br></li><li>Hit the ground running with Microsoft’s lightweight, open source .NET Core platform, including ASP.NET Core MVC, ASP.NET Core web services, and Entity Framework Core<br></li><li>Find complete coverage of XAML, .NET 4.7, and Visual Studio 2017<br></li><li>Understand the philosophy behind .NET and the new, cross-platform alternative, .NET Core</li></ul>", false, null, "https://images.springer.com/sgw/books/medium/9781484230176.jpg", 0, 1372, false, new DateTime(2017, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://www.apress.com/gp/book/9781484230176", null, "https://images.springer.com/sgw/books/medium/9781484230176.jpg", "Pro C# 7", null, null },
                    { 9, "Bipin Joshi", 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "<p>Master the basics of XML as well as the namespaces and objects you need to know in order to work efficiently with XML.You’ll learn extensive support for XML in everything from data access to configuration, from raw parsing to code documentation.You will see clear, practical examples that illustrate best practices in implementing XML APIs and services as part of your C#-based Windows 10 applications.<br><i>Beginning XML with C# 7 </i>is completely revised to cover the XML features of .NET Framework 4.7 using C# 7 programming language. In this update, you’ll discover the tight integration of XML with ADO.NET and LINQ as well as additional .NET support for today’s RESTful web services and Web API.<br>Written by a Microsoft Most Valuable Professional and developer, this book demystifies everything to do with XML and C# 7.<br><b>What You Will Learn:</b></p><ul><li>Discover how XML works with the .NET Framework<br></li><li>Read, write, access, validate, and manipulate XML documents<br></li><li>Transform XML with XSLT<br></li><li>Use XML serialization and web services<br></li><li>Combine XML in ADO.NET and SQL Server<br></li><li>Create services using Windows Communication Foundation<br></li><li>Work with LINQ<br></li><li>Use XML with Web API and more<br></li></ul><b>Who This Book Is For :</b>Those with experience in C# and .NET new to the nuances of using XML.&nbsp; Some XML experience is helpful.", false, null, "https://images.springer.com/sgw/books/medium/9781484231043.jpg", 0, 453, false, new DateTime(2017, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://www.apress.com/gp/book/9781484231043", null, "https://images.springer.com/sgw/books/medium/9781484231043.jpg", "Beginning XML with C# 7", null, null },
                    { 10, "Zhimin Zhan", 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "<p>Solve your Selenium WebDriver problems with this quick guide to automated testing of web applications with Selenium WebDriver in C#. <i>Selenium WebDriver Recipes in C#, Second Edition </i>contains hundreds of solutions to real-world problems, with clear explanations and ready - to - run Selenium test scripts that you can use in your own projects.</p> <p>You'll learn:</p> <ul> <li>How to locate web elements and test functions for hyperlinks, buttons, TextFields and TextAreas, radio buttons, CheckBoxes, and more</li> <li>How to use Selenium WebDriver for select lists, navigation, assertions, frames, file upload and pop-up dialogs</li> <li>How to debug test scripts and test data</li> <li>How to manage and deal with browser profiles and capabilities&lt;</li> <li>How to manage tests for advanced user interactions and experiences(UX)</li> <li>How to work with and manage tests and testing using Selenium Remote Control and Selenium Server</li></ul><br>AudienceThis book is for experienced.NET and C# Windows application programmers/developers.", false, null, "https://images.springer.com/sgw/books/medium/9781484217412.jpg", 0, 164, false, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://www.apress.com/gp/book/9781484217412", null, "https://images.springer.com/sgw/books/medium/9781484217412.jpg", "Selenium WebDriver Recipes in C#", null, null }
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "Id", "CreatedTime", "CreatedUser", "Enrolled", "Joined", "UpdatedTime", "UpdatedUser", "UserId" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "e29790bd-b712-4594-8b3f-c13cbc2943ac" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AvatarId",
                table: "AspNetUsers",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ImageId",
                table: "Books",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ThumbnailId",
                table: "Books",
                column: "ThumbnailId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_PlaylistId",
                table: "Chapters",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_ProfileId",
                table: "Enrollments",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_ChapterId",
                table: "Episodes",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_CategoryId",
                table: "Playlists",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_ImageId",
                table: "Playlists",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_OwnerId",
                table: "Playlists",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_ThumbnailId",
                table: "Playlists",
                column: "ThumbnailId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchHistories_ProfileId",
                table: "WatchHistories",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "WatchHistories");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Thumbnails");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Avatars");
        }
    }
}
