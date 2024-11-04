using CellPhone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CellPhone.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private readonly CellPhoneContext _context = new CellPhoneContext();
        private readonly StatusMessageModel _statusMessage = new StatusMessageModel();

        public async Task<ActionResult> Index()
        {
            var orders = await _context.Orders
                                    .Include(o => o.Customer)
                                    .OrderByDescending(o => o.Order_at)
                                    .ToListAsync();
            foreach(var order in orders)
            {
                order.OrderItems = await _context.OrderItems.Include(oi => oi.Product).Where(oi => oi.OrderId == order.OrderId).ToListAsync();
            }
            return View(orders);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Bill(string id)
        {
            try
            {
                var order = _context.Orders
                                    .Include(o => o.Customer)
                                    .FirstOrDefault(o => o.OrderId == id) ?? throw new Exception("Đơn hàng không tồn tại");
                order.OrderItems = _context.OrderItems.Include(oi => oi.Product).Where(oi => oi.OrderId == id).ToList();
                return View(order);
            }
            catch (Exception ex)
            {
                _statusMessage.Success = false;
                _statusMessage.Title = ex.Message;

                TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Payment(string id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id) ?? throw new Exception("Đơn hàng không tồn tại");
                if (order.IsPayment) throw new Exception("Đơn hàng đã được thanh toán");
                order.IsPayment = true;
                order.Payment_at = DateTime.Now;
                try
                {
                    _context.Orders.AddOrUpdate(order);
                    await _context.SaveChangesAsync();
                }
                catch { }

                _statusMessage.Success = true;
                _statusMessage.Title = "Xuất hoá đơn và thanh toán thành công";
            }
            catch (Exception ex)
            {
                _statusMessage.Success = false;
                _statusMessage.Title = "Xuất hoá đơn và thanh toán không thành công";
                _statusMessage.Message = ex.Message;
            }

            TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Return(string id)
        {
            try
            {
                var order = await _context.Orders.Include(o => o.Customer)
                                                    .FirstOrDefaultAsync(o => o.OrderId == id) ?? throw new Exception("Đơn hàng không tồn tại");
                order.OrderItems = _context.OrderItems.Include(oi => oi.Product).Where(oi => oi.OrderId == id).ToList();
                if (!order.IsPayment) throw new Exception("Đơn hàng chưa thanh toán");
                return Json(order);
            }
            catch (Exception ex)
            {

            }

            return View();
        }
    }
}