using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CellPhone.Areas.User.Models
{
    public class ResetPasswordViewModel
    {
        [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự")]
        [Required(ErrorMessage = "Phải có {0}")]
        [DataType(DataType.EmailAddress, ErrorMessage = "{0} không đúng định dạng")]
        public string Email { get; set; }

        [DisplayName("Mật khẩu")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự")]
        [Required(ErrorMessage = "Phải có {0}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Xác nhận mật khẩu")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự")]
        [Required(ErrorMessage = "Phải có {0}")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không trùng khớp")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; }
    }
}