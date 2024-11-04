using CellPhone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CellPhone.Areas.Admin.Models.Order
{
    public class OrderViewModel
    {
        public List<ProductOrderViewModel> ProductOrders { get; set; }
        public DiscountViewModel Discount { get; set; }
        public string CustomerId { get; set; }
        public double PaidMoney { get; set; }
    }

    public class ProductOrderViewModel
    {
        public CellPhone.Models.Product Product { get; set; }
        public int Quantity { get; set; }
    }

    public class DiscountViewModel
    {
        public string DiscountType { get; set; }
        public int DiscountQuantity { get; set; }
    }
}