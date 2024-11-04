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
    public class ForgotPasswordController : Controller
    {
        private readonly CellPhoneContext _context = new CellPhoneContext();
        private readonly UserRepository _userRepository = new UserRepository();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(ForgotPasswordViewModel forgotPassword)
        {
            var statusMessage = new StatusMessageModel();
            try
            {
                if (ModelState.IsValid)
                {  
                    var email = forgotPassword.Email;
                    var user = _context.Users.FirstOrDefault(u => u.Email == email);
                    if (user == null) throw new Exception("Tài khoản chưa được đăng ký");
                    var host = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port;
                    await _userRepository.ForgotPassword(user, Url, host);
                    statusMessage.Success = true;
                    statusMessage.Title = "Kiểm tra Email để lấy lại mật khẩu";
                }
            }
            catch (Exception ex)
            {
                statusMessage.Success = false;
                statusMessage.Title = "Lấy lại mật khẩu không thành công";
                statusMessage.Message = ex.Message;
            }

            TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
            return View();
        }

        [HttpGet()]
        public ActionResult Confirm(string token, string email)
        {
            if (token == null || email == null)
            {
                var statusMessage = new StatusMessageModel
                {
                    Success = false,
                    Title = "Đặt lại mật khẩu thất bại"
                };

                TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
                return Redirect("/");
            }

            ViewBag.Token = token;
            ViewBag.Email = email;
            return View();
        }

        [HttpPost()]
        public async Task<ActionResult> Confirm(ResetPasswordViewModel resetPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var statusMessage = new StatusMessageModel();
                    var user = _context.Users.FirstOrDefault(u => u.Email == resetPassword.Email) ?? throw new Exception("Tài khoản chưa được đăng ký");
                    await _userRepository.ResetPassword(user, resetPassword.Token, resetPassword.Password);
                    statusMessage = new StatusMessageModel
                    {
                        Success = true,
                        Title = "Đặt lại mật khẩu thành công",
                    };

                    TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
                }
                else throw new Exception();
            }
            catch (Exception ex)
            {
                var statusMessage = new StatusMessageModel
                {
                    Success = false,
                    Title = "Đặt lại mật khẩu thất bại",
                    Message = ex.Message
                };

                TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
            }

            return Redirect("/");
        }
    }
}