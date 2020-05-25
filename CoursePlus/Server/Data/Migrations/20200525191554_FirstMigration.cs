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
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    ImageId = table.Column<int>(nullable: true),
                    ThumbnailPath = table.Column<string>(nullable: true),
                    ThumbnailId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    CreatedUser = table.Column<string>(nullable: true),
                    UpdatedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quizzes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quizzes_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quizzes_Thumbnails_ThumbnailId",
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
                    { "1002a5ed-a8e4-4c5c-9587-b8a8e1aa320b", "9d0ed9a1-83a4-44b8-8de6-25e3d82dd1e9", "Admin", "ADMIN" },
                    { "0a3d93c9-e5d8-4ed5-b79d-d1e6a3768228", "7ff46a67-2016-40cb-ab9d-3cbe2594018e", "User", "USER" }
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
                    { 26, "Web" },
                    { 25, "Vie quotidienne" },
                    { 24, "Tourisme" },
                    { 23, "Télévision" },
                    { 22, "Sports" },
                    { 21, "Sciences" },
                    { 20, "Pour adultes" },
                    { 19, "Payx du monde" },
                    { 18, "Nature" },
                    { 17, "Musique" },
                    { 16, "Loisirs" },
                    { 15, "Littérature" },
                    { 13, "Informatique" },
                    { 12, "Histoire" },
                    { 11, "Géographie" },
                    { 10, "Gastronomie" },
                    { 9, "Culture générale" },
                    { 8, "Cinéma" },
                    { 7, "Célébrités" },
                    { 6, "Bande dessinée" },
                    { 5, "Arts" },
                    { 4, "Architecture" },
                    { 3, "Music" },
                    { 2, "Archéologie" },
                    { 14, "OpenBSD" },
                    { 1, "Animaux" }
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
                table: "Profiles",
                columns: new[] { "Id", "CreatedTime", "CreatedUser", "Enrolled", "Joined", "UpdatedTime", "UpdatedUser", "UserId" },
                values: new object[] { 1, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com", "e29790bd-b712-4594-8b3f-c13cbc2943ac" });

            migrationBuilder.InsertData(
                table: "Quizzes",
                columns: new[] { "Id", "CategoryId", "CreatedTime", "CreatedUser", "Description", "ImageId", "ImagePath", "ThumbnailId", "ThumbnailPath", "Title", "UpdatedTime", "UpdatedUser" },
                values: new object[,]
                {
                    { 3, 1, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com", "...", null, "Data/Quiz/Images/Animaux/animaux-celebres.png", null, "Data/Quiz/Thumbnails/Animaux/animaux-celebres.png", "Animaux célèbres", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com" },
                    { 4, 1, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com", "...", null, "Data/Quiz/Images/Animaux/animaux-en-chiffres.png", null, "Data/Quiz/Thumbnails/Animaux/animaux-en-chiffres.png", "Animaux en chiffres", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com" },
                    { 5, 1, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com", "...", null, "Data/Quiz/Images/Animaux/animaux-en-tout-genre.png", null, "Data/Quiz/Thumbnails/Animaux/animaux-en-tout-genre.png", "Animaux en tout genre", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com" },
                    { 6, 1, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com", "...", null, "Data/Quiz/Images/Animaux/animaux-et-habitat.png", null, "Data/Quiz/Thumbnails/Animaux/animaux-et-habitat.png", "Animaux et habitat", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com" },
                    { 7, 1, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com", "...", null, "Data/Quiz/Images/Animaux/chevaux.png", null, "Data/Quiz/Thumbnails/Animaux/chevaux.png", "Chevaux", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com" },
                    { 8, 1, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com", "...", null, "Data/Quiz/Images/Animaux/dragon.png", null, "Data/Quiz/Thumbnails/Animaux/dragon.png", "Dragon", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com" },
                    { 9, 1, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com", "...", null, "Data/Quiz/Images/Animaux/fourmis.png", null, "Data/Quiz/Thumbnails/Animaux/fourmis.png", "Fourmis", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com" },
                    { 10, 1, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com", "...", null, "Data/Quiz/Images/Animaux/nos-amis-les-chats.png", null, "Data/Quiz/Thumbnails/Animaux/nos-amis-les-chats.png", "Nos amis les chats", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com" },
                    { 11, 1, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com", "...", null, "Data/Quiz/Images/Animaux/oiseaux.png", null, "Data/Quiz/Thumbnails/Animaux/oiseaux.png", "Oiseaux", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com" },
                    { 12, 1, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com", "...", null, "Data/Quiz/Images/Animaux/requins.png", null, "Data/Quiz/Thumbnails/Animaux/requins.png", "Requins", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com" },
                    { 2, 2, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com", "...", null, "Data/Quiz/Images/Archeologie/machu-pichu.png", null, "Data/Quiz/Thumbnails/Archeologie/machu-pichu.png", "Machu Pichu", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com" },
                    { 1, 4, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com", "...", null, "Data/Quiz/Images/Architecture/chateau-de-chambord.png", null, "Data/Quiz/Thumbnails/Architecture/chateau-de-chambord.png", "Chateau de Chambord", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "azur.consult@gmail.com" }
                });

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
                name: "IX_Quizzes_CategoryId",
                table: "Quizzes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_ImageId",
                table: "Quizzes",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_ThumbnailId",
                table: "Quizzes",
                column: "ThumbnailId");

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
                name: "Quizzes");

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
