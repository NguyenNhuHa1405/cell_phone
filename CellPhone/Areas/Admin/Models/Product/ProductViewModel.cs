using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CellPhone.Areas.Admin.Models.Product
{
    public class ProductViewModel
    {
        public string ProductId { get; set; }

        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = "Phải có {0}")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự")]
        public string ProductName { get; set; }

        [DisplayName("Giá sản phẩm")]
        [Required(ErrorMessage = "Phải có {0}")]
        public double ProductPrice { get; set; }

        public string ProductSlug { get; set; }

        [DisplayName("Danh mục")]
        [Required(ErrorMessage = "Phải có {0}")]
        public string CategoryId { get; set; }

        [DisplayName("Mô tả sản phẩm")]
        [Required(ErrorMessage = "Phải có {0}")]
        public string ProductDescription { get; set; }
    }
}