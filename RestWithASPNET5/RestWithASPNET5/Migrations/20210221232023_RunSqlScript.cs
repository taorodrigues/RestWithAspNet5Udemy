using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RestWithASPNET5.Migrations
{
  public partial class RunSqlScript : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.Sql(File.ReadAllText("Migrations\\V1__Full_Dump_SQL_Server.sql"));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
  }
}