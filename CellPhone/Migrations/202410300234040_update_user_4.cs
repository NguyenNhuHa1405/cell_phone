namespace CellPhone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_user_4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RolePermissions1",
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
                "dbo.UserRoles1",
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
            DropForeignKey("dbo.UserRoles1", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles1", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePermissions1", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePermissions1", "PermissonId", "dbo.Permissions");
            DropIndex("dbo.UserRoles1", new[] { "UserId" });
            DropIndex("dbo.UserRoles1", new[] { "RoleId" });
            DropIndex("dbo.RolePermissions1", new[] { "PermissonId" });
            DropIndex("dbo.RolePermissions1", new[] { "RoleId" });
            DropTable("dbo.UserRoles1");
            DropTable("dbo.RolePermissions1");
        }
    }
}
