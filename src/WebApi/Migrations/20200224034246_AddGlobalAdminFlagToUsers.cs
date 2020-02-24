using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class AddGlobalAdminFlagToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PassageReference_Groups_GroupId",
                table: "PassageReference");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PassageReference",
                table: "PassageReference");

            migrationBuilder.RenameTable(
                name: "PassageReference",
                newName: "PassageReferences");

            migrationBuilder.RenameIndex(
                name: "IX_PassageReference_GroupId",
                table: "PassageReferences",
                newName: "IX_PassageReferences_GroupId");

            migrationBuilder.AddColumn<bool>(
                name: "IsGlobalAdmin",
                table: "UserSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PassageReferences",
                table: "PassageReferences",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PassageReferences_Groups_GroupId",
                table: "PassageReferences",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PassageReferences_Groups_GroupId",
                table: "PassageReferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PassageReferences",
                table: "PassageReferences");

            migrationBuilder.DropColumn(
                name: "IsGlobalAdmin",
                table: "UserSettings");

            migrationBuilder.RenameTable(
                name: "PassageReferences",
                newName: "PassageReference");

            migrationBuilder.RenameIndex(
                name: "IX_PassageReferences_GroupId",
                table: "PassageReference",
                newName: "IX_PassageReference_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PassageReference",
                table: "PassageReference",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PassageReference_Groups_GroupId",
                table: "PassageReference",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
