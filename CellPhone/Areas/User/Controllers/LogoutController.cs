using CellPhone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CellPhone.Areas.User.Controllers
{
    public class LogoutController : Controller
    {
        private readonly StatusMessageModel _statusMessage = new StatusMessageModel();
        // GET: Admin/Logout
        public ActionResult Index(string redirectUrl)
        {
            Session.Remove("user");
            _statusMessage.Success = true;
            _statusMessage.Title = "Đăng xuất thành công";
            TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
            return Redirect(redirectUrl ?? "/");
        }
    }
}