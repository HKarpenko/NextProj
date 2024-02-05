using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextProj.Migrations
{
    /// <inheritdoc />
    public partial class AddEventDayRecurrenceStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DayRecurrences",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<long>(type: "bigint", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayRecurrences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayRecurrences_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DayRecurrences2DayPositions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayPosition = table.Column<int>(type: "int", nullable: false),
                    EventDayRecurrenceId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayRecurrences2DayPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayRecurrences2DayPositions_DayRecurrences_EventDayRecurrenceId",
                        column: x => x.EventDayRecurrenceId,
                        principalTable: "DayRecurrences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayRecurrences_EventId",
                table: "DayRecurrences",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_DayRecurrences2DayPositions_EventDayRecurrenceId",
                table: "DayRecurrences2DayPositions",
                column: "EventDayRecurrenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayRecurrences2DayPositions");

            migrationBuilder.DropTable(
                name: "DayRecurrences");
        }
    }
}
