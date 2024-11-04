namespace CellPhone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderItem
    {
        [StringLength(450)]
        public string OrderItemId { get; set; }

        [StringLength(450)]
        public string ProductId { get; set; }

        [StringLength(450)]
        public string OrderId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? OrderItem_at { get; set; }

        public int Quantity { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
