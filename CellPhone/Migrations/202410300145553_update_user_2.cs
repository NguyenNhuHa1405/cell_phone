namespace CellPhone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_user_2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RoleClaims", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserTokens", "UserId", "dbo.Users");
            DropIndex("dbo.RoleClaims", new[] { "RoleId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserTokens", new[] { "UserId" });
            DropColumn("dbo.Users", "NormalizedUserName");
            DropColumn("dbo.Users", "NormalizedEmail");
            DropColumn("dbo.Users", "SecurityStamp");
            DropColumn("dbo.Users", "ConcurrencyStamp");
            DropColumn("dbo.Users", "TwoFactorEnabled");
            DropTable("dbo.RoleClaims");
            DropTable("dbo.UserClaims");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserTokens");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserTokens",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 450),
                        LoginProvider = c.String(nullable: false, maxLength: 450),
                        Name = c.String(nullable: false, maxLength: 450),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.Name });
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 450),
                        ProviderKey = c.String(nullable: false, maxLength: 450),
                        ProviderDisplayName = c.String(),
                        UserId = c.String(nullable: false, maxLength: 450),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey });
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        UserId = c.String(nullable: false, maxLength: 450),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        RoleId = c.String(nullable: false, maxLength: 450),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "TwoFactorEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "ConcurrencyStamp", c => c.String());
            AddColumn("dbo.Users", "SecurityStamp", c => c.String());
            AddColumn("dbo.Users", "NormalizedEmail", c => c.String(maxLength: 256));
            AddColumn("dbo.Users", "NormalizedUserName", c => c.String(maxLength: 256));
            CreateIndex("dbo.UserTokens", "UserId");
            CreateIndex("dbo.UserLogins", "UserId");
            CreateIndex("dbo.UserClaims", "UserId");
            CreateIndex("dbo.RoleClaims", "RoleId");
            AddForeignKey("dbo.UserTokens", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserLogins", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserClaims", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RoleClaims", "RoleId", "dbo.Roles", "Id", cascadeDelete: true);
        }
    }
}
