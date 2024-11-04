using CellPhone.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CellPhone.Areas.Admin.Models
{
    public class EditUserViewModel
    {
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }

        [DisplayName("Tên đăng nhập")]
        [Required(ErrorMessage = "Phải có {0}")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} phải có độ dài từ {2} tới {1} ký từ")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Phải là {0}")]
        [Required(ErrorMessage = "Phải có {0}")]
        public string Email { get; set; }

        [DisplayName("Số điện thoại")]
        [RegularExpression(@"(84|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Phải là {0}")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        /*public bool TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public bool Active { get; set; }

        public int City { get; set; }

        public int District { get; set; }

        public int Ward { get; set; }

        public string Address { get; set; }*/
    }
}