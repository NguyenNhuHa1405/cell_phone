using System.ComponentModel.DataAnnotations.Schema;

namespace CellPhone.Models
{
    [Table("UserRoles")]
    public class UserRole
    {
        public string RoleId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}