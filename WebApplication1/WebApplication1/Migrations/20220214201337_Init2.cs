using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //            migrationBuilder.Sql(@"INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Address], [City]) VALUES (N'a5149c24-d12e-412d-b00d-08d9efe55efd', N'bogdan@example123.com', N'BOGDAN@EXAMPLE123.COM', NULL, NULL, 0, N'AQAAAAEAACcQAAAAEK7C0xon/9MBwO+y5nMR9tb/52K+SYON5PWMOQpRWXVu0j+aKxqJRDzSBnS9l3oUZg==', N'DERYV4RO7VUWTZGMS5KDXDAYJNMKUWQ5', N'e0af50a0-2f95-4982-b933-bd3110a5450d', N'admin', 0, 0, NULL, 1, 0, N'admin', N'admin')");
            //            migrationBuilder.Sql(@"INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'3359da00-6856-422a-06bb-08d9efe63720', N'admin', N'ADMIN', N'df7d9e32-0a1a-4cd6-9e4f-a7c80b8889f3')
            //");
            //            migrationBuilder.Sql(@"INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'a5149c24-d12e-412d-b00d-08d9efe55efd', N'3359da00-6856-422a-06bb-08d9efe63720')
            //");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
