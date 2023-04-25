using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class WebAppInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    UserId = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    Username = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    ReceiverId = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    ReceiverUsername = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    PiorityScore = table.Column<int>(type: "int", nullable: false),
                    Restaurant = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    Detail = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    ReceiveLocation = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    IsTaken = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    Username = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    Password = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    UserImg = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    FistName = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    LastName = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Success = table.Column<int>(type: "int", nullable: false),
                    Failed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
