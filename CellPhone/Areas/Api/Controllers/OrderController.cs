using CellPhone.Areas.Admin.Models.Order;
using CellPhone.Datas;
using CellPhone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CellPhone.Areas.Api.Controllers
{
    public class OrderController : Controller
    {
        private readonly CellPhoneContext _context = new CellPhoneContext();

        [HttpGet]
        public ActionResult CheckCustomer(string queryCustomer)
        {
            try
            {
                var customers = _context.Customers.Where(c => c.CustomerName.Contains(queryCustomer));
                var result = new
                {
                    success = true,
                    datas = customers
                };

                if (!customers.Any()) throw new Exception("Không có dữ liệu");
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(JsonConvert.SerializeObject(result, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new
                {
                    success = false,
                    message = ex.Message,
                };
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult CheckProduct(string queryProduct)
        {
            try
            {
                var products = _context.Products.Where(p => p.ProductName.Contains(queryProduct));
                var result = new
                {
                    success = true,
                    datas = products
                };

                if (!products.Any()) throw new Exception("Không có dữ liệu");
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(JsonConvert.SerializeObject(result, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new
                {
                    success = false,
                    message = ex.Message,
                };
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Index(OrderViewModel orderModel)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(orderModel.CustomerId) ?? throw new Exception("Khách hàng không tồn tại");
                if (orderModel.ProductOrders == null || orderModel.ProductOrders.Count <= 0) throw new Exception("Chưa có sản phẩm trong giỏ hàng");
                var orderItems = new List<OrderItem>();
                var order = new Order
                {
                    OrderId = Guid.NewGuid().ToString(),
                    CustomerId = customer.CustomerId,
                    Order_at = DateTime.Now
                };
                var provisional = 0.0;
                foreach (var productOrder in orderModel.ProductOrders)
                {
                    provisional += (productOrder.Quantity * productOrder.Product.ProductPrice);
                    var product = await _context.Products.FindAsync(productOrder.Product.ProductId) ?? throw new Exception("Sản phẩm không tồn tại");
                    if (productOrder.Quantity <= 0) throw new Exception("Số lượng phải lớn hơn 1");
                    orderItems.Add(
                        new OrderItem
                        {
                            OrderItemId = Guid.NewGuid().ToString(),
                            ProductId = product.ProductId,
                            Quantity = productOrder.Quantity,
                            OrderId = order.OrderId,
                            OrderItem_at = DateTime.Now,
                        }
                    );
                }

                order.Total = provisional;
                if (orderModel.Discount != null)
                {
                    switch (orderModel.Discount.DiscountType)
                    {
                        case DiscountType.Percent:
                            order.DiscountType = DiscountType.Percent;
                            if (orderModel.Discount.DiscountQuantity < 0 || orderModel.Discount.DiscountQuantity > 100) throw new Exception("Giám giá không hợp lệ");
                            order.DiscountAmount = orderModel.Discount.DiscountQuantity;
                            order.Total = provisional * (100 - orderModel.Discount.DiscountQuantity) / 100;
                            break;
                        case DiscountType.Money:
                            order.DiscountType = DiscountType.Money;
                            if (orderModel.Discount.DiscountQuantity < 0 || orderModel.Discount.DiscountQuantity > provisional) throw new Exception("Giám giá không hợp lệ");
                            order.DiscountAmount = orderModel.Discount.DiscountQuantity;
                            order.Total = provisional - orderModel.Discount.DiscountQuantity;
                            break;
                    }
                }

                order.TotalWithoutDiscount = provisional;
                order.PaidMoney = orderModel.PaidMoney;
                order.Debt = order.Total - order.PaidMoney;
                try
                {

                    _context.Orders.Add(order);
                    _context.OrderItems.AddRange(orderItems);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    throw new Exception("Không thể tạo hoá đơn");
                }

                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(JsonConvert.SerializeObject(new
                {
                    Success = true,
                    Title = "Tạo hoá đơn thành công",
                    Order = order
                }, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Success = false,
                    Title = "Tạo hoá đơn không thành công",
                    ex.Message
                });
            }
        }
    }
}