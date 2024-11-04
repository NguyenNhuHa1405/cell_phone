namespace CellPhone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {

        [StringLength(450)]
        public string ProductId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductName { get; set; }

        public double ProductPrice { get; set; }

        [StringLength(450)]
        public string ProductSlug { get; set; }

        [Required]
        [StringLength(450)]
        public string CategoryId { get; set; }

        [Required]
        public string ProductDescription { get; set; }

        public virtual Category Category { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
