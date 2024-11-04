using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CellPhone.Areas.User.Models
{
    public class LoginViewModel
    {
        [DisplayName("Tài khoản hoặc Email")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự")]
        [Required(ErrorMessage = "Phải có {0}")]
        public string UserNameOrEmail { get; set; }

        [DisplayName("Mật khẩu")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự")]
        [Required(ErrorMessage = "Phải có {0}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}