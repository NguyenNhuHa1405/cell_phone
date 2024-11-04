using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CellPhone.Models
{
    public class CreateUserViewModel {
        [DisplayName("Tên đăng nhập")]
        [Required(ErrorMessage = "Phải có {0}")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} phải có độ dài từ {2} tới {1} ký tự")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Phải là {0}")]
        [Required(ErrorMessage = "Phải có {0}")]
        public string Email { get; set; }

        [DisplayName("Mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Phải có {0}")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự")]
        public string Password { get; set; }

        [DisplayName("Số điện thoại")]
        [RegularExpression(@"(84|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Phải là {0}")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    
        [DisplayName("Thành phố")]
        public int City { get; set; }

        [DisplayName("Quận huyện")]
        public int District { set; get; }

        [DisplayName("Phường xã")]
        public int Ward { set; get; }

        [DisplayName("Địa chỉ cụ thể")]
        public string Address { get; set; }
    }
}