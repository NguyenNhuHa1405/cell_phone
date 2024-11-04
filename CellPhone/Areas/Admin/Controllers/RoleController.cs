using CellPhone.Areas.Admin.Models.Role;
using CellPhone.Datas;
using CellPhone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CellPhone.Areas.Admin.Controllers
{
    public class RoleController : Controller
    {
        private readonly CellPhoneContext _context = new CellPhoneContext();
        private readonly StatusMessageModel _statusMessage = new StatusMessageModel();

        public async Task<ActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            return View(roles);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var role = new Role
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = roleViewModel.RoleName,
                        NormalizedName = roleViewModel.RoleNormalizedName
                    };

                    if (await _context.Roles.AnyAsync(r => r.NormalizedName == role.NormalizedName)) throw new Exception("Quyền đã tồn tại");
                    _context.Roles.Add(role);
                    await _context.SaveChangesAsync();
                    _statusMessage.Success = true;
                    _statusMessage.Title = "Tạo quyền thành công";
                    TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
                    return RedirectToAction(nameof(Create));
                }
            }
            catch (Exception ex)
            {
                _statusMessage.Success = false;
                _statusMessage.Title = "Tạo quyền không thành công";
                _statusMessage.Message = ex.Message;
            }

            TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
            return View();
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            try
            {
                var role = _context.Roles.Find(id) ?? throw new Exception("Quyền không tồn tại");
                if (role.NormalizedName == RoleData.Admin) throw new Exception("Không thể xoá quyền này");
                _context.Roles.Remove(role);
                _context.SaveChanges();
                _statusMessage.Success = true;
                _statusMessage.Title = "Xoá quyền thành công";
            }
            catch (Exception ex)
            {
                _statusMessage.Success = false;
                _statusMessage.Title = "Xoá quyền không thành công";
                _statusMessage.Message = ex.Message;
            }

            TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Permission(string id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id) ?? throw new Exception("Quyền không tồn tại");
                return View(role);
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
        public async Task<ActionResult> Permission(string id, RolePermissionViewModel rolePermissionViewModel)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id) ?? throw new Exception("Quyền không tồn tại");
                var rolePermissions = new List<RolePermission>();
                _context.RolePermissions.RemoveRange(_context.RolePermissions.Where(rp => rp.RoleId == role.Id));
                foreach (var permissionId in rolePermissionViewModel.PermissionIds)
                {
                    var permission = await _context.Permissions.FindAsync(permissionId) ?? throw new Exception("Phạm vi không tồn tại");
                    rolePermissions.Add(new RolePermission
                    {
                        RoleId = role.Id,
                        PermissonId = permissionId
                    });
                }

                _context.RolePermissions.AddRange(rolePermissions);
                await _context.SaveChangesAsync();
                _statusMessage.Success = true;
                _statusMessage.Title = "Thêm phạm vi thành công";
            }
            catch (Exception ex)
            {
                _statusMessage.Success = false;
                _statusMessage.Title = "Thêm phạm vi không thành công";
                _statusMessage.Message = ex.Message;
            }

            TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
            return RedirectToAction(nameof(Index));
        }
    }
}