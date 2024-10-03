using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporateBankingApp.Migrations
{
    /// <inheritdoc />
    public partial class ver1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientCounter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CounterValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientCounter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileDetails",
                columns: table => new
                {
                    FileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateUploaded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDetails", x => x.FileId);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankKycs",
                columns: table => new
                {
                    BankKycId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxpayerIdentificationNumberFileId = table.Column<int>(type: "int", nullable: false),
                    LicenseRegulatorApprovalsOrLicenseAgreementFileId = table.Column<int>(type: "int", nullable: false),
                    FinancialStatementsFileId = table.Column<int>(type: "int", nullable: false),
                    AnnualReportsFileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankKycs", x => x.BankKycId);
                    table.ForeignKey(
                        name: "FK_BankKycs_FileDetails_AnnualReportsFileId",
                        column: x => x.AnnualReportsFileId,
                        principalTable: "FileDetails",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BankKycs_FileDetails_FinancialStatementsFileId",
                        column: x => x.FinancialStatementsFileId,
                        principalTable: "FileDetails",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BankKycs_FileDetails_LicenseRegulatorApprovalsOrLicenseAgreementFileId",
                        column: x => x.LicenseRegulatorApprovalsOrLicenseAgreementFileId,
                        principalTable: "FileDetails",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BankKycs_FileDetails_TaxpayerIdentificationNumberFileId",
                        column: x => x.TaxpayerIdentificationNumberFileId,
                        principalTable: "FileDetails",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ClientKyc",
                columns: table => new
                {
                    ClientKycId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CINNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PanNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PowerOfAttorneyFileId = table.Column<int>(type: "int", nullable: false),
                    BankAccessFileId = table.Column<int>(type: "int", nullable: false),
                    MOUFileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientKyc", x => x.ClientKycId);
                    table.ForeignKey(
                        name: "FK_ClientKyc_FileDetails_BankAccessFileId",
                        column: x => x.BankAccessFileId,
                        principalTable: "FileDetails",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClientKyc_FileDetails_MOUFileId",
                        column: x => x.MOUFileId,
                        principalTable: "FileDetails",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClientKyc_FileDetails_PowerOfAttorneyFileId",
                        column: x => x.PowerOfAttorneyFileId,
                        principalTable: "FileDetails",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SuperAdmins",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserLoginId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperAdmins", x => x.AdminId);
                    table.ForeignKey(
                        name: "FK_SuperAdmins_UserLogins_UserLoginId",
                        column: x => x.UserLoginId,
                        principalTable: "UserLogins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    BankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankIFSCCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankKycId = table.Column<int>(type: "int", nullable: true),
                    UserLoginId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankId);
                    table.ForeignKey(
                        name: "FK_Banks_BankKycs_BankKycId",
                        column: x => x.BankKycId,
                        principalTable: "BankKycs",
                        principalColumn: "BankKycId");
                    table.ForeignKey(
                        name: "FK_Banks_UserLogins_UserLoginId",
                        column: x => x.UserLoginId,
                        principalTable: "UserLogins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    BlockedFunds = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FounderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserLoginId = table.Column<int>(type: "int", nullable: true),
                    ClientKycId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    BankAccountAccountId = table.Column<int>(type: "int", nullable: true),
                    BeneficiaryLists = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    isBeneficiaryOutbound = table.Column<bool>(type: "bit", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_Clients_BankAccounts_BankAccountAccountId",
                        column: x => x.BankAccountAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_Clients_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "BankId");
                    table.ForeignKey(
                        name: "FK_Clients_ClientKyc_ClientKycId",
                        column: x => x.ClientKycId,
                        principalTable: "ClientKyc",
                        principalColumn: "ClientKycId");
                    table.ForeignKey(
                        name: "FK_Clients_UserLogins_UserLoginId",
                        column: x => x.UserLoginId,
                        principalTable: "UserLogins",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankAccountAccountId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_BankAccounts_BankAccountAccountId",
                        column: x => x.BankAccountAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_BankId",
                table: "BankAccounts",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BankKycs_AnnualReportsFileId",
                table: "BankKycs",
                column: "AnnualReportsFileId");

            migrationBuilder.CreateIndex(
                name: "IX_BankKycs_FinancialStatementsFileId",
                table: "BankKycs",
                column: "FinancialStatementsFileId");

            migrationBuilder.CreateIndex(
                name: "IX_BankKycs_LicenseRegulatorApprovalsOrLicenseAgreementFileId",
                table: "BankKycs",
                column: "LicenseRegulatorApprovalsOrLicenseAgreementFileId");

            migrationBuilder.CreateIndex(
                name: "IX_BankKycs_TaxpayerIdentificationNumberFileId",
                table: "BankKycs",
                column: "TaxpayerIdentificationNumberFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_BankKycId",
                table: "Banks",
                column: "BankKycId");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_UserLoginId",
                table: "Banks",
                column: "UserLoginId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientKyc_BankAccessFileId",
                table: "ClientKyc",
                column: "BankAccessFileId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientKyc_MOUFileId",
                table: "ClientKyc",
                column: "MOUFileId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientKyc_PowerOfAttorneyFileId",
                table: "ClientKyc",
                column: "PowerOfAttorneyFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_BankAccountAccountId",
                table: "Clients",
                column: "BankAccountAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_BankId",
                table: "Clients",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientKycId",
                table: "Clients",
                column: "ClientKycId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_UserLoginId",
                table: "Clients",
                column: "UserLoginId");

            migrationBuilder.CreateIndex(
                name: "IX_SuperAdmins_UserLoginId",
                table: "SuperAdmins",
                column: "UserLoginId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BankAccountAccountId",
                table: "Transactions",
                column: "BankAccountAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientCounter");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "SuperAdmins");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "ClientKyc");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "BankKycs");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "FileDetails");
        }
    }
}
