namespace CellPhone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_user_7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 450),
                        PermissonId = c.String(nullable: false, maxLength: 450),
                    })
                .PrimaryKey(t => new { t.RoleId, t.PermissonId })
                .ForeignKey("dbo.Permissions", t => t.PermissonId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.PermissonId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 450),
                        UserId = c.String(nullable: false, maxLength: 450),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePermissions", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePermissions", "PermissonId", "dbo.Permissions");
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.RolePermissions", new[] { "PermissonId" });
            DropIndex("dbo.RolePermissions", new[] { "RoleId" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.RolePermissions");
        }
    }
}
