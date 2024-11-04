using CellPhone.Areas.User.Models;
using CellPhone.Models;
using CellPhone.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.UI.WebControls;

namespace CellPhone.Repositories
{
    public class UserRepository
    {
        private readonly MailService _emailSender = new MailService();
        private readonly CellPhoneContext _context = new CellPhoneContext();

        public async Task Login(User user, string password, HttpSessionStateBase session)
        {
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) throw new Exception("Tài khoản hoặc mật khẩu không chính xác");
            await GenerateSession(user, session);
        }

        public async Task Register(User user, string password, HttpSessionStateBase session)
        {
            user.Id = Guid.NewGuid().ToString();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, 12);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            await GenerateSession(user, session);
        }

        public async Task GenerateSession(User user, HttpSessionStateBase session)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Sid, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            if(user.UserRoles != null)
            {
                foreach (var userRole in user.UserRoles)
                {
                    var role = await _context.Roles.FindAsync(userRole.RoleId);
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }
            }
            session.Add("user", claims);
        }

        public async Task ForgotPassword(User user, UrlHelper urlHelper, string host)
        {
            var email = user.Email;
            var token = GenerateToken(user);
            var confirmUrl = urlHelper.Action("Confirm", new { token, email });
            var body = $@"
                <div>
                    <h3>Hi {user.UserName}</h3>
                    <span>
                        Vui lòng click <a href='{host + confirmUrl}'>vào đây</a> để đặt lại mật khẩu
                    </span>
                </div>
            ";

            await _emailSender.SendEmailAsync(user.Email, "Đặt lại mật khẩu", body);
        }

        public async Task ResetPassword(User user, string token, string password)
        {
            if (!BCrypt.Net.BCrypt.Verify(user.Id, token)) throw new Exception("Token không hợp lệ");
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, 12);
            user.PasswordHash = passwordHash;
            _context.Users.AddOrUpdate(user);
            await _context.SaveChangesAsync();
        }

        public string GenerateToken(User user)
        {
            return BCrypt.Net.BCrypt.HashPassword(user.Id, 12);
        }
    }
}