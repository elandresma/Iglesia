using Microsoft.EntityFrameworkCore.Migrations;

namespace Iglesia.Web.Migrations
{
    public partial class Correctthedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Professions_ProfessionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Assistances_Meetings_MeetingId",
                table: "Assistances");

            migrationBuilder.DropForeignKey(
                name: "FK_Assistances_AspNetUsers_UserId",
                table: "Assistances");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Churches_ChurchId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Districts_Name_RegionId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Churches_Name_DistrictId",
                table: "Churches");

            migrationBuilder.AlterColumn<int>(
                name: "ChurchId",
                table: "Meetings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "Districts",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DistrictId",
                table: "Churches",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Assistances",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MeetingId",
                table: "Assistances",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProfessionId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Name_RegionId",
                table: "Districts",
                columns: new[] { "Name", "RegionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Churches_Name_DistrictId",
                table: "Churches",
                columns: new[] { "Name", "DistrictId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Professions_ProfessionId",
                table: "AspNetUsers",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assistances_Meetings_MeetingId",
                table: "Assistances",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assistances_AspNetUsers_UserId",
                table: "Assistances",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Churches_ChurchId",
                table: "Meetings",
                column: "ChurchId",
                principalTable: "Churches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Professions_ProfessionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Assistances_Meetings_MeetingId",
                table: "Assistances");

            migrationBuilder.DropForeignKey(
                name: "FK_Assistances_AspNetUsers_UserId",
                table: "Assistances");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Churches_ChurchId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Districts_Name_RegionId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Churches_Name_DistrictId",
                table: "Churches");

            migrationBuilder.AlterColumn<int>(
                name: "ChurchId",
                table: "Meetings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "Districts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "DistrictId",
                table: "Churches",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Assistances",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "MeetingId",
                table: "Assistances",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ProfessionId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Name_RegionId",
                table: "Districts",
                columns: new[] { "Name", "RegionId" },
                unique: true,
                filter: "[RegionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Churches_Name_DistrictId",
                table: "Churches",
                columns: new[] { "Name", "DistrictId" },
                unique: true,
                filter: "[DistrictId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Professions_ProfessionId",
                table: "AspNetUsers",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assistances_Meetings_MeetingId",
                table: "Assistances",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assistances_AspNetUsers_UserId",
                table: "Assistances",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Churches_ChurchId",
                table: "Meetings",
                column: "ChurchId",
                principalTable: "Churches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
