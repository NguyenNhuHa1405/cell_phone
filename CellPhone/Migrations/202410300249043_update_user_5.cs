namespace CellPhone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_user_5 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RolePermissions1", newName: "RolePermissions");
            RenameTable(name: "dbo.UserRoles1", newName: "UserRoles");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.RolePermissions", "PermissionId", "dbo.Permissions");
            DropForeignKey("dbo.RolePermissions", "RoleId", "dbo.Roles");
            DropIndex("dbo.RolePermissions", new[] { "PermissionId" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.RolePermissions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        PermissionId = c.String(nullable: false, maxLength: 450),
                        RoleId = c.String(nullable: false, maxLength: 450),
                    })
                .PrimaryKey(t => new { t.PermissionId, t.RoleId });
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 450),
                        UserId = c.String(nullable: false, maxLength: 450),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId });
            
            CreateIndex("dbo.RolePermissions", "PermissionId");
            AddForeignKey("dbo.RolePermissions", "RoleId", "dbo.Roles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RolePermissions", "PermissionId", "dbo.Permissions", "PermissionId", cascadeDelete: true);
            AddForeignKey("dbo.UserRoles", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.UserRoles", newName: "UserRoles1");
            RenameTable(name: "dbo.RolePermissions", newName: "RolePermissions1");
        }
    }
}
