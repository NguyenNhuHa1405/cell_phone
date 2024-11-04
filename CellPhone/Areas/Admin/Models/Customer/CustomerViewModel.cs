using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CellPhone.Areas.Admin.Models.Customer
{
    public class CustomerViewModel
    {
        [DisplayName("Tên khách hàng")]
        [Required(ErrorMessage = "Phải có {0}")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự")]
        public string CustomerName { get; set; }

        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "Phải có {0}")]
        [RegularExpression(@"(84|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Phải là {0}")]
        public string CustomerPhone { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Phải có {0}")]
        [EmailAddress(ErrorMessage = "Phải là {0}")]
        public string CustomerEmail { get; set; }

        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage = "Phải có {0}")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự")]
        public string CustomerAddress { get; set; }
    }
}