using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountService.Projections.Api.Migrations
{
    public partial class AddAccountDetailAndListProjections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Api");

            migrationBuilder.CreateTable(
                name: "AccountDetails",
                schema: "Api",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NameDutch = table.Column<string>(maxLength: 200, nullable: true),
                    NameFrench = table.Column<string>(maxLength: 200, nullable: true),
                    NameEnglish = table.Column<string>(maxLength: 200, nullable: true),
                    NameGerman = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountDetails", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "AccountList",
                schema: "Api",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountList", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "ProjectionStates",
                schema: "Api",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Position = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectionStates", x => x.Name)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountDetails_NameDutch",
                schema: "Api",
                table: "AccountDetails",
                column: "NameDutch")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountList_Name",
                schema: "Api",
                table: "AccountList",
                column: "Name")
                .Annotation("SqlServer:Clustered", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountDetails",
                schema: "Api");

            migrationBuilder.DropTable(
                name: "AccountList",
                schema: "Api");

            migrationBuilder.DropTable(
                name: "ProjectionStates",
                schema: "Api");
        }
    }
}
