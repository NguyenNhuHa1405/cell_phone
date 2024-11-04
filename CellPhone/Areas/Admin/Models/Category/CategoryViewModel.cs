using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CellPhone.Areas.Admin.Models.Category
{
    public class CategoryViewModel
    {
        public string CategoryId { get; set; }

        [DisplayName("Tên danh mục")]
        [Required(ErrorMessage = "Phải có {0}")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự")]
        public string CategoryName { get; set; }

        [DisplayName("Mô tả danh mục")]
        [Required(ErrorMessage = "Phải có {0}")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} ký tự")]
        public string CategoryDescription { get; set; }

        [DisplayName("Danh mục cha")]
        public string ParentCategoryId { get; set; }

        public string CategorySlug { get; set; }
    }
}