namespace CellPhone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {

        [StringLength(450)]
        public string OrderId { get; set; }

        [StringLength(450)]
        public string CustomerId { get; set; }

        public bool IsPayment { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Payment_at { get; set; }

        public string DiscountType { get; set; }

        public double? DiscountAmount { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Order_at { get; set; }

        public double? TotalWithoutDiscount { get; set; }

        public double? PaidMoney { get; set; }

        public double? Debt { get; set; }

        public double? Total { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
