namespace HelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttachmentIssues",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FileName = c.String(),
                    Attachment = c.Binary(),
                    Issue_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Issues", t => t.Issue_Id)
                .Index(t => t.Issue_Id);

            CreateTable(
                "dbo.CategoryIssues",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Issues",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Date = c.DateTime(nullable: false),
                    Title = c.String(),
                    Description = c.String(),
                    Prioritet_Id = c.Int(nullable: false),
                    ServiceUser_Id = c.String(maxLength: 128),
                    Status_Id = c.Int(nullable: false),
                    User_Id = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prioritets", t => t.Prioritet_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ServiceUser_Id)
                .ForeignKey("dbo.Status", t => t.Status_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Prioritet_Id)
                .Index(t => t.ServiceUser_Id)
                .Index(t => t.Status_Id)
                .Index(t => t.User_Id);

            CreateTable(
                "dbo.Prioritets",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    CodeName = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    UserName = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(nullable: false, maxLength: 128),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                {
                    LoginProvider = c.String(nullable: false, maxLength: 128),
                    ProviderKey = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    RoleId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.Status",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    CodeName = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AspNetRoles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Name = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.IssueCategoryIssues",
                c => new
                {
                    Issue_Id = c.Int(nullable: false),
                    CategoryIssue_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Issue_Id, t.CategoryIssue_Id })
                .ForeignKey("dbo.Issues", t => t.Issue_Id, cascadeDelete: true)
                .ForeignKey("dbo.CategoryIssues", t => t.CategoryIssue_Id, cascadeDelete: true)
                .Index(t => t.Issue_Id)
                .Index(t => t.CategoryIssue_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Issues", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Issues", "Status_Id", "dbo.Status");
            DropForeignKey("dbo.Issues", "ServiceUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Issues", "Prioritet_Id", "dbo.Prioritets");
            DropForeignKey("dbo.IssueCategoryIssues", "CategoryIssue_Id", "dbo.CategoryIssues");
            DropForeignKey("dbo.IssueCategoryIssues", "Issue_Id", "dbo.Issues");
            DropForeignKey("dbo.AttachmentIssues", "Issue_Id", "dbo.Issues");
            DropIndex("dbo.IssueCategoryIssues", new[] { "CategoryIssue_Id" });
            DropIndex("dbo.IssueCategoryIssues", new[] { "Issue_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Issues", new[] { "User_Id" });
            DropIndex("dbo.Issues", new[] { "Status_Id" });
            DropIndex("dbo.Issues", new[] { "ServiceUser_Id" });
            DropIndex("dbo.Issues", new[] { "Prioritet_Id" });
            DropIndex("dbo.AttachmentIssues", new[] { "Issue_Id" });
            DropTable("dbo.IssueCategoryIssues");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Status");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Prioritets");
            DropTable("dbo.Issues");
            DropTable("dbo.CategoryIssues");
            DropTable("dbo.AttachmentIssues");
        }

    }
}
