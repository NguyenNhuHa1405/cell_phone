using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CellPhone.Models
{
    [Table("RolePermissions")]
    public class RolePermission
    {
        public string RoleId { get; set; }
        public string PermissonId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [ForeignKey("PermissonId")]
        public Permission Permission { get; set; }
    }
}