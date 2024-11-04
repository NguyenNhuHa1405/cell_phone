using BCrypt.Net;
using CellPhone.Areas.Admin.Models;
using CellPhone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CellPhone.Areas.Admin.Controllers
{
    /*[Authorize(RoleData.Admin)]*/
    public class UserController : Controller {
        private readonly CellPhoneContext _context = new CellPhoneContext();
        private readonly StatusMessageModel _statusMessage = new StatusMessageModel();

        public async Task<ActionResult> Index() {
            var users = await _context.Users.ToListAsync();
            ViewBag.Users = users;
            return View();
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateUserViewModel createUser) {
            try {
                if(ModelState.IsValid) {
                    if (await _context.Users.AnyAsync(u => u.UserName == createUser.UserName)) throw new Exception("Tên đăng nhập đã tồn tại");
                    if (await _context.Users.AnyAsync(u => u.Email == createUser.Email)) throw new Exception("Email đã tồn tại");
                    if (await _context.Users.AnyAsync(u => u.PhoneNumber == createUser.PhoneNumber)) throw new Exception("Số điện thoại đã tồn tại");
                    var newUser = JsonConvert.DeserializeObject<CellPhone.Models.User>(JsonConvert.SerializeObject(createUser));
                    newUser.Active = true;
                    newUser.Id = Guid.NewGuid().ToString();
                    newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUser.Password, 12);
                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();
                    _statusMessage.Success = true;
                    _statusMessage.Title = "Tạo tài khoản thành công";
                    TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
                    return RedirectToAction(nameof(Create));
                }
            }catch(Exception ex) {
                _statusMessage.Success = false;
                _statusMessage.Title = "Tạo tài khoản không thành công";
                _statusMessage.Message = ex.Message;
                TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
            }

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id) ?? throw new Exception("Tài khoản không tồn tại");
                var editUser = JsonConvert.DeserializeObject<EditUserViewModel>(JsonConvert.SerializeObject(user));
                return View(editUser);
            }
            catch (Exception ex)
            {
                _statusMessage.Success = false;
                _statusMessage.Title = "Chỉnh sửa tài khoản không thành công";
                _statusMessage.Message = ex.Message;
                TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, EditUserViewModel editUser)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var user = await _context.Users.FindAsync(id) ?? throw new Exception("Tài khoản không tồn tại");
                    if(editUser.Password != null)
                    {
                        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(editUser.Password, 12);
                    }

                    if (await _context.Users.AnyAsync(u => u.UserName == editUser.UserName && u.UserName != user.UserName)) throw new Exception("Tên đăng nhập đã tồn tại");
                    if (await _context.Users.AnyAsync(u => u.Email == editUser.Email && u.Email != user.Email)) throw new Exception("Email đã tồn tại");
                    if (await _context.Users.AnyAsync(u => u.PhoneNumber == editUser.PhoneNumber && u.PhoneNumber != user.PhoneNumber)) throw new Exception("Số điện thoại đã tồn tại");

                    user.UserName = editUser.UserName;
                    user.PhoneNumber = editUser.PhoneNumber;
                    user.Email = editUser.Email;
                    try
                    {
                        _context.Users.AddOrUpdate(user);
                        _context.SaveChanges();
                    }catch { throw new Exception("Không thể chỉnh sửa"); }

                    _statusMessage.Success = true;
                    _statusMessage.Title = "Chỉnh sửa tài khoản thành công";
                }
            }
            catch(Exception ex)
            {
                _statusMessage.Success = false;
                _statusMessage.Title = "Chỉnh sửa tài khoản không thành công";
                _statusMessage.Message = ex.Message;
            }

            TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id) ?? throw new Exception("Tài khoản không tồn tại");
                try
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    throw new Exception("Không thể xoá tài khoản");
                }

                _statusMessage.Success = true;
                _statusMessage.Title = "Xoá tài khoản thành công";
            }
            catch (Exception ex)
            {
                _statusMessage.Success = false;
                _statusMessage.Title = "Xoá tài khoản không thành công";
                _statusMessage.Message = ex.Message;
            }

            TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> AddRole(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id) ?? throw new Exception("Tài khoản không tồn tại");
                return View(user);
            }
            catch (Exception ex)
            {
                _statusMessage.Success = false;
                _statusMessage.Title = ex.Message;
            }

            TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> AddRole(string id, List<string> RoleIds)
        {
            try
            {
                var user = await _context.Users.FindAsync(id) ?? throw new Exception("Tài khoản không tồn tại");
                var userRoles = new List<UserRole>();
                _context.UserRoles.RemoveRange(_context.UserRoles.Where(ur => ur.UserId == id));
                if (RoleIds != null)
                {
                    foreach (var roleId in RoleIds)
                    {
                        var role = await _context.Roles.FindAsync(roleId) ?? throw new Exception("Quyền không tồn tại");
                        userRoles.Add(new UserRole
                        {
                            UserId = id,
                            RoleId = roleId
                        });
                    }
                }

                _context.UserRoles.AddRange(userRoles);
                await _context.SaveChangesAsync();
                _statusMessage.Success = true;
                _statusMessage.Title = "Thêm quyền thành công";
            }
            catch (Exception ex)
            {
                _statusMessage.Success = false;
                _statusMessage.Title = "Thêm quyền không thành công";
                _statusMessage.Message = ex.Message;
            }

            TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
            return RedirectToAction(nameof(Index));
        }
    }
}