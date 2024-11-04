using CellPhone.Areas.Admin.Models.Customer;
using CellPhone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CellPhone.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CellPhoneContext _context = new CellPhoneContext();
        private readonly StatusMessageModel statusMessage = new StatusMessageModel();
       
        public async Task<ActionResult> Index()
        {
            var customers = await _context.Customers.Include(c => c.Orders).ToListAsync();
            return View(customers);
        }

        public ActionResult Create(string redirectUrl)
        {
            ViewBag.RedirectUrl = redirectUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CustomerViewModel customerViewModel, string redirectUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var customer = JsonConvert.DeserializeObject<Customer>(JsonConvert.SerializeObject(customerViewModel));
                    customer.CustomerId = Guid.NewGuid().ToString();
                    if (_context.Customers.Any(c => c.CustomerPhone == customer.CustomerPhone)) throw new Exception("Số điện thoại đã tồn tại");
                    if (_context.Customers.Any(c => c.CustomerEmail == customer.CustomerEmail)) throw new Exception("Email đã tồn tại");
                    try
                    {
                        _context.Customers.Add(customer);
                        await _context.SaveChangesAsync();
                    }
                    catch
                    {
                        throw new Exception("Không thể thêm khách hàng");
                    }

                    statusMessage.Success = true;
                    statusMessage.Title = "Thêm khách hàng thành công";
                    TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
                    if (redirectUrl == null) return RedirectToAction(nameof(Create));
                    return Redirect(redirectUrl);
                }
            }
            catch (Exception ex)
            {
                statusMessage.Success = false;
                statusMessage.Title = "Thêm khách hàng không thành công";
                statusMessage.Message = ex.Message;
            }

            TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
            ViewBag.RedirectUrl = redirectUrl;
            return View();
        }
    }
}