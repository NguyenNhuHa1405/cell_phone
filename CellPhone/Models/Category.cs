namespace CellPhone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
        }

        [StringLength(450)]
        public string CategoryId { get; set; }

        [StringLength(450)]
        public string CategorySlug { get; set; }

        [Required]
        [StringLength(200)]
        public string CategoryName { get; set; }

        [Required]
        [StringLength(200)]
        public string CategoryDescription { get; set; }

        [StringLength(450)]
        public string ParentCategoryId { get; set; }

        public virtual List<Category> ChildCategories { get; set; }

        public virtual Category ParentCategory { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
