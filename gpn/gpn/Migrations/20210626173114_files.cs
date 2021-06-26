using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace gpn.Migrations
{
    public partial class files : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OpertationTypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpertationTypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    number = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    comapnyID = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    state = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    parentID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Equipmen__FD291E4095624219", x => x.number);
                    table.ForeignKey(
                        name: "FK__Equipment__comap__440B1D61",
                        column: x => x.comapnyID,
                        principalTable: "Companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Equipment__paren__44FF419A",
                        column: x => x.parentID,
                        principalTable: "Equipment",
                        principalColumn: "number",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comapnyID = table.Column<int>(type: "int", nullable: true),
                    role = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                    table.ForeignKey(
                        name: "FK__User__comapnyID__4D94879B",
                        column: x => x.comapnyID,
                        principalTable: "Companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SlaRules",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    typeID = table.Column<int>(type: "int", nullable: true),
                    nextTypeID = table.Column<int>(type: "int", nullable: true),
                    optionType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    duration = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlaRules", x => x.id);
                    table.ForeignKey(
                        name: "FK__SlaRules__nextTy__4CA06362",
                        column: x => x.nextTypeID,
                        principalTable: "OpertationTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__SlaRules__typeID__4BAC3F29",
                        column: x => x.typeID,
                        principalTable: "OpertationTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    equipmentNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    typeId = table.Column<int>(type: "int", nullable: true),
                    location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    date = table.Column<DateTime>(type: "datetime", nullable: true),
                    performer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DocId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    postponedTime = table.Column<long>(type: "bigint", nullable: true),
                    fileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.id);
                    table.ForeignKey(
                        name: "FK__Operation__equip__48CFD27E",
                        column: x => x.equipmentNumber,
                        principalTable: "Equipment",
                        principalColumn: "number",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Operation__typeI__49C3F6B7",
                        column: x => x.typeId,
                        principalTable: "OpertationTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Operations_Files_fileId",
                        column: x => x.fileId,
                        principalTable: "Files",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArrivalOperation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    logisticCompany = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArrivalOperation", x => x.id);
                    table.ForeignKey(
                        name: "FK__ArrivalOpera__id__4AB81AF0",
                        column: x => x.id,
                        principalTable: "Operations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OperationDeadlines",
                columns: table => new
                {
                    equipmentNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    typeID = table.Column<int>(type: "int", nullable: false),
                    operationID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Operatio__9EF368CB25B1225D", x => new { x.equipmentNumber, x.typeID });
                    table.ForeignKey(
                        name: "FK__Operation__equip__45F365D3",
                        column: x => x.equipmentNumber,
                        principalTable: "Equipment",
                        principalColumn: "number",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Operation__opera__46E78A0C",
                        column: x => x.operationID,
                        principalTable: "Operations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Operation__typeI__47DBAE45",
                        column: x => x.typeID,
                        principalTable: "OpertationTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_comapnyID",
                table: "Equipment",
                column: "comapnyID");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_parentID",
                table: "Equipment",
                column: "parentID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationDeadlines_operationID",
                table: "OperationDeadlines",
                column: "operationID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationDeadlines_typeID",
                table: "OperationDeadlines",
                column: "typeID");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_equipmentNumber",
                table: "Operations",
                column: "equipmentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_fileId",
                table: "Operations",
                column: "fileId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_typeId",
                table: "Operations",
                column: "typeId");

            migrationBuilder.CreateIndex(
                name: "IX_SlaRules_nextTypeID",
                table: "SlaRules",
                column: "nextTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SlaRules_typeID",
                table: "SlaRules",
                column: "typeID");

            migrationBuilder.CreateIndex(
                name: "IX_User_comapnyID",
                table: "User",
                column: "comapnyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArrivalOperation");

            migrationBuilder.DropTable(
                name: "OperationDeadlines");

            migrationBuilder.DropTable(
                name: "SlaRules");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "OpertationTypes");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
