using CellPhone.Areas.User.Models;
using CellPhone.Models;
using CellPhone.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CellPhone.Areas.User.Controllers
{
    public class RegisterController : Controller
    {
        private readonly CellPhoneContext _context = new CellPhoneContext();
        private readonly UserRepository _userRepository = new UserRepository();

        [HttpGet]
        public ActionResult Index()
        {
            if (Session["user"] != null) return Redirect("/");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(RegisterViewModel register)
        {
            if (Session["user"] != null) return Redirect("/");
            try
            {
                if (ModelState.IsValid)
                {
                    var newUser = JsonConvert.DeserializeObject<CellPhone.Models.User>(JsonConvert.SerializeObject(register));
                    var user = _context.Users.FirstOrDefault(u => u.Email == newUser.Email || u.UserName == newUser.UserName);
                    if (user != null) throw new Exception("Tài khoản đã tồn tại");
                    await _userRepository.Register(newUser, register.Password, Session);
                    var statusMessage = new StatusMessageModel
                    {
                        Success = true,
                        Title = "Đăng ký thành công",
                    };

                    TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
                    return Redirect("/");
                }
            }
            catch (Exception ex)
            {
                var statusMessage = new StatusMessageModel
                {
                    Success = false,
                    Title = "Đăng ký không thành công",
                    Message = ex.Message
                };

                TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
            }
            return View();
        }
    }
}