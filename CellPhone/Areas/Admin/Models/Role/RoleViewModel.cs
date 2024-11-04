using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CellPhone.Areas.Admin.Models.Role
{
    public class RoleViewModel
    {
        [DisplayName("Tên hiển thị")]
        [Required(ErrorMessage = "Phải có {0}")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự")]
        public string RoleNormalizedName { get; set; }

        [DisplayName("Tên quyền")]
        [Required(ErrorMessage = "Phải có {0}")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự")]
        public string RoleName { get; set; }
    }
}