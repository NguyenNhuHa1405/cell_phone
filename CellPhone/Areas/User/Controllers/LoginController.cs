using CellPhone.Areas.User.Models;
using CellPhone.Models;
using CellPhone.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CellPhone.Areas.User.Controllers
{
    public class LoginController : Controller
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
        public async Task<ActionResult> Index(LoginViewModel login)
        {
            if (Session["user"] != null) return Redirect("/");
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _context.Users
                        .Include(u => u.UserRoles)
                        .FirstOrDefaultAsync(u => u.Email == login.UserNameOrEmail || u.UserName == login.UserNameOrEmail);
                    if (user == null) throw new Exception("Tài khoản hoặc mật khẩu không chính xác");
                    await _userRepository.Login(user, login.Password, Session);
                    var statusMessage = new StatusMessageModel
                    {
                        Success = true,
                        Title = "Đăng nhập thành công"
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
                    Title = "Đăng nhập không thành công",
                    Message = ex.Message
                };

                TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
            }

            return View();
        }
    }
}