using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class createTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Counties",
                columns: table => new
                {
                    countyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    countyTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counties", x => x.countyId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    departmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    departmentTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.departmentId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nationalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userPass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    country = table.Column<int>(type: "int", nullable: false),
                    department = table.Column<int>(type: "int", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    education = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tel = table.Column<long>(type: "bigint", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    maritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employmentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    workingStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "Employments",
                columns: table => new
                {
                    EmploymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmploymentTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employments", x => x.EmploymentId);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    genderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    genderTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.genderId);
                });

            migrationBuilder.CreateTable(
                name: "Maritals",
                columns: table => new
                {
                    maritalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maritalTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maritals", x => x.maritalId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "transferDocuments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userUploaderArea = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userUploaderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    roleUploader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userReceiverArea = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userReceiverId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    roleReceiver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProvinceOrigin = table.Column<int>(type: "int", nullable: false),
                    CountyOrigin = table.Column<int>(type: "int", nullable: false),
                    DistrictOrigin = table.Column<int>(type: "int", nullable: false),
                    ProvinceDestination = table.Column<int>(type: "int", nullable: false),
                    CountyDestination = table.Column<int>(type: "int", nullable: false),
                    DistrictDestination = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fileFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dataBytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsAllowed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transferDocuments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CountyLevels",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nationalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userPass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    department = table.Column<int>(type: "int", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    education = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tel = table.Column<long>(type: "bigint", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    maritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employmentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    workingStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeesuserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountyLevels", x => x.userId);
                    table.ForeignKey(
                        name: "FK_CountyLevels_Employees_EmployeesuserId",
                        column: x => x.EmployeesuserId,
                        principalTable: "Employees",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "DistrictLevels",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nationalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userPass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    county = table.Column<int>(type: "int", nullable: false),
                    department = table.Column<int>(type: "int", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    education = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tel = table.Column<long>(type: "bigint", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    maritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employmentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    workingStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeesuserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistrictLevels", x => x.userId);
                    table.ForeignKey(
                        name: "FK_DistrictLevels_Employees_EmployeesuserId",
                        column: x => x.EmployeesuserId,
                        principalTable: "Employees",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "ProvinceLevel",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nationalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userPass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    department = table.Column<int>(type: "int", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    education = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tel = table.Column<long>(type: "bigint", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    maritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employmentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    workingStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EmployeesuserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvinceLevel", x => x.userId);
                    table.ForeignKey(
                        name: "FK_ProvinceLevel_Employees_EmployeesuserId",
                        column: x => x.EmployeesuserId,
                        principalTable: "Employees",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "documentUploadCountyLevels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    roleUploader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ownerUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fileFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dataBytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documentUploadCountyLevels", x => x.id);
                    table.ForeignKey(
                        name: "FK_documentUploadCountyLevels_CountyLevels_userId",
                        column: x => x.userId,
                        principalTable: "CountyLevels",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "documentUploadDistrictLevels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    roleUploader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ownerUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    county = table.Column<int>(type: "int", nullable: false),
                    department = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fileFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dataBytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documentUploadDistrictLevels", x => x.id);
                    table.ForeignKey(
                        name: "FK_documentUploadDistrictLevels_DistrictLevels_userId",
                        column: x => x.userId,
                        principalTable: "DistrictLevels",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "documentUploadProvinceLevels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    roleUploader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ownerUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fileFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dataBytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documentUploadProvinceLevels", x => x.id);
                    table.ForeignKey(
                        name: "FK_documentUploadProvinceLevels_ProvinceLevel_userId",
                        column: x => x.userId,
                        principalTable: "ProvinceLevel",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Counties",
                columns: new[] { "countyId", "countyTitle" },
                values: new object[,]
                {
                    { 1, "شعبه1" },
                    { 2, "شعبه2" },
                    { 3, "شعبه3" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "departmentId", "departmentTitle" },
                values: new object[,]
                {
                    { 1, "شعبه1" },
                    { 2, "شعبه2" },
                    { 3, "شعبه3" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "userId", "IsDelete", "RegisterDate", "address", "area", "country", "dateOfBirth", "department", "education", "employmentStatus", "fName", "gender", "lName", "maritalStatus", "nationalCode", "role", "tel", "userPass", "workingStatus" },
                values: new object[] { 1, false, new DateTime(2023, 8, 24, 0, 47, 29, 707, DateTimeKind.Local).AddTicks(7908), "کرج", "استان", 0, "1377/02/20", 1, "کارشناسی", "رسمی", "اشکان", "مرد", "مطهری", "مجرد", "0021047022", "مدیریت", 9351225600L, "123", "فعال" });

            migrationBuilder.InsertData(
                table: "Employments",
                columns: new[] { "EmploymentId", "EmploymentTitle" },
                values: new object[,]
                {
                    { 1, "رسمی" },
                    { 2, "پیمانی" },
                    { 3, "قراردادی" },
                    { 4, "آزمایشی" }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "genderId", "genderTitle" },
                values: new object[,]
                {
                    { 1, "مرد" },
                    { 2, "زن" }
                });

            migrationBuilder.InsertData(
                table: "Maritals",
                columns: new[] { "maritalId", "maritalTitle" },
                values: new object[,]
                {
                    { 1, "مجرد" },
                    { 2, "متاهل" }
                });

            migrationBuilder.InsertData(
                table: "ProvinceLevel",
                columns: new[] { "userId", "EmployeesuserId", "IsDelete", "RegisterDate", "address", "dateOfBirth", "department", "education", "employmentStatus", "fName", "gender", "lName", "maritalStatus", "nationalCode", "role", "tel", "userPass", "workingStatus" },
                values: new object[] { 1, null, false, new DateTime(2023, 8, 24, 0, 47, 29, 707, DateTimeKind.Local).AddTicks(7759), "کرج", "1377/02/20", 1, "کارشناسی", "رسمی", "اشکان", "مرد", "مطهری", "مجرد", "0021047022", "مدیریت", 9351225600L, "123", "فعال" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "IsDelete", "RoleTitle" },
                values: new object[,]
                {
                    { 1, false, "مدیریت" },
                    { 2, false, "معاونت فناوری اطلاعات" },
                    { 3, false, "معاونت سیاسی" },
                    { 4, false, "معاونت فرهنگی" },
                    { 5, false, "معاونت اجتماعی" },
                    { 6, false, "معاونت مالی" },
                    { 7, false, "معاونت امور فرا سطح" },
                    { 8, false, "معاونت توسعه و پشتیبانی" },
                    { 9, false, "معاونت منابع انسانی" },
                    { 10, false, "معاونت حراست" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountyLevels_EmployeesuserId",
                table: "CountyLevels",
                column: "EmployeesuserId");

            migrationBuilder.CreateIndex(
                name: "IX_DistrictLevels_EmployeesuserId",
                table: "DistrictLevels",
                column: "EmployeesuserId");

            migrationBuilder.CreateIndex(
                name: "IX_documentUploadCountyLevels_userId",
                table: "documentUploadCountyLevels",
                column: "userId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_documentUploadDistrictLevels_userId",
                table: "documentUploadDistrictLevels",
                column: "userId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_documentUploadProvinceLevels_userId",
                table: "documentUploadProvinceLevels",
                column: "userId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProvinceLevel_EmployeesuserId",
                table: "ProvinceLevel",
                column: "EmployeesuserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Counties");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "documentUploadCountyLevels");

            migrationBuilder.DropTable(
                name: "documentUploadDistrictLevels");

            migrationBuilder.DropTable(
                name: "documentUploadProvinceLevels");

            migrationBuilder.DropTable(
                name: "Employments");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Maritals");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "transferDocuments");

            migrationBuilder.DropTable(
                name: "CountyLevels");

            migrationBuilder.DropTable(
                name: "DistrictLevels");

            migrationBuilder.DropTable(
                name: "ProvinceLevel");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
