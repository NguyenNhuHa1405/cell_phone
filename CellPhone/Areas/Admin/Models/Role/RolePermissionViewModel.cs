using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CellPhone.Areas.Admin.Models.Role
{
    public class RolePermissionViewModel
    {
        [DisplayName("Danh sách phạm vi truy cập")]
        [Required(ErrorMessage = "Phải có {0}")]
        public List<string> PermissionIds { get; set; } = new List<string>();

        public string RoleId { get; set; }
    }
}