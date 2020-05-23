using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlus.Server.Data.Migrations
{
    public partial class AddSomeBooksA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "CreatedTime", "CreatedUser", "Description", "Featured", "ImageId", "ImageUrl", "Language", "PageCount", "Popular", "PublishingDate", "PurchaseLink", "ThumbnailId", "ThumbnailUrl", "Title", "UpdatedTime", "UpdatedUser" },
                values: new object[] { 4, "Dmitri Nesteruk", 28, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "<p>Apply modern C++17 to the implementations of classic design patterns. As well as covering traditional design patterns, this book fleshes out new patterns and approaches that will be useful to C++ developers. The author presents concepts as a fun investigation of how problems can be solved in different ways, along the way using varying degrees of technical sophistication and explaining different sorts of trade-offs.<br><i>Design Patterns in Modern C++ </i>also provides a technology demo for modern C++, showcasing how some of its latest features (e.g., coroutines) make difficult problems a lot easier to solve. The examples in this book are all suitable for putting into production, with only a few simplifications made in order to aid readability.<br><b>What You Will Learn</b><br></p><ul><li>Apply design patterns to modern C++ programming<br></li><li>Use creational patterns of builder, factories, prototype and singleton<br></li><li>Implement structural patterns such as adapter, bridge, decorator, facade and more<br></li><li>Work with the behavioral patterns such as chain of responsibility, command, iterator, mediator and more<br></li><li>Apply functional design patterns such as Monad and more<br></li></ul><b><br></b><b>Who This Book Is For</b><br>Those with at least some prior programming experience, especially in C++.", false, null, "https://images.springer.com/sgw/books/medium/9781484236024.jpg", 0, 314, false, new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://www.apress.com/gp/book/9781484236024", null, "https://images.springer.com/sgw/books/medium/9781484236024.jpg", "Design Patterns in Modern C++", null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
