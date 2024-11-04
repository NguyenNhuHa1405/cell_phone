namespace CellPhone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_user_3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RolePermissions1", "PermissonId", "dbo.Permissions");
            DropForeignKey("dbo.RolePermissions1", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserRoles1", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserRoles1", "UserId", "dbo.Users");
            DropIndex("dbo.RolePermissions1", new[] { "RoleId" });
            DropIndex("dbo.RolePermissions1", new[] { "PermissonId" });
            DropIndex("dbo.UserRoles1", new[] { "RoleId" });
            DropIndex("dbo.UserRoles1", new[] { "UserId" });
            DropTable("dbo.RolePermissions1");
            DropTable("dbo.UserRoles1");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserRoles1",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 450),
                        UserId = c.String(nullable: false, maxLength: 450),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId });
            
            CreateTable(
                "dbo.RolePermissions1",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 450),
                        PermissonId = c.String(nullable: false, maxLength: 450),
                    })
                .PrimaryKey(t => new { t.RoleId, t.PermissonId });
            
            CreateIndex("dbo.UserRoles1", "UserId");
            CreateIndex("dbo.UserRoles1", "RoleId");
            CreateIndex("dbo.RolePermissions1", "PermissonId");
            CreateIndex("dbo.RolePermissions1", "RoleId");
            AddForeignKey("dbo.UserRoles1", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserRoles1", "RoleId", "dbo.Roles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RolePermissions1", "RoleId", "dbo.Roles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RolePermissions1", "PermissonId", "dbo.Permissions", "PermissionId", cascadeDelete: true);
        }
    }
}
