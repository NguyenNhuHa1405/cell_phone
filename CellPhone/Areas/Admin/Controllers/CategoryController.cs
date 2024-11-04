using CellPhone.Areas.Admin.Models.Category;
using CellPhone.Attributes;
using CellPhone.Datas;
using CellPhone.Models;
using CellPhone.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CellPhone.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CellPhoneContext _context = new CellPhoneContext();

        [Authorization(PermissionData.Read)]
        public async Task<ActionResult> Index()
        {
            var categories = await _context.Categories.Include(c => c.ParentCategory).ToListAsync();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpGet]
        [Authorization(PermissionData.Create)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorization(PermissionData.Create)]
        public async Task<ActionResult> Create(CategoryViewModel createCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!(await _context.Categories.AnyAsync(c => c.CategoryId == createCategory.ParentCategoryId))) throw new Exception("Danh mục cha không tồn tại");
                    var category = JsonConvert.DeserializeObject<Category>(JsonConvert.SerializeObject(createCategory));
                    category.CategoryId = Guid.NewGuid().ToString();
                    category.CategorySlug = AppUtilities.GenerateSlug(createCategory.CategoryName);
                    if (await _context.Categories.AnyAsync(c => c.CategorySlug == category.CategorySlug)) throw new Exception("Danh mục đã tồn tại");
                    try
                    {

                        _context.Categories.Add(category);
                        await _context.SaveChangesAsync();
                    }
                    catch
                    {
                        throw new Exception("Không thể thêm dữ liệu");
                    }

                    var statusMessage = new StatusMessageModel
                    {
                        Success = true,
                        Title = "Thêm danh mục thành công",
                    };

                    TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
                    return RedirectToAction("Create");
                }
                else
                {
                    foreach (var model in ModelState.Values)
                    {
                        foreach (var error in model.Errors)
                        {
                            throw new Exception(error.ErrorMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var statusMessage = new StatusMessageModel
                {
                    Success = false,
                    Title = "Thêm danh mục không thành công",
                    Message = ex.Message
                };

                TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
            }

            return View();
        }

        [HttpGet]
        [Authorization(PermissionData.Delete)]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id) ?? throw new Exception("Danh mục không tồn tại");
                try
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    throw new Exception("Xoá thất bại");
                }

                var statusMessage = new StatusMessageModel
                {
                    Success = true,
                    Title = "Xoá danh mục thành công",
                };

                TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
            }
            catch (Exception ex)
            {
                var statusMessage = new StatusMessageModel
                {
                    Success = false,
                    Title = "Xoá danh mục không thành công",
                    Message = ex.Message
                };

                TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorization(PermissionData.Update)]
        public async Task<ActionResult> Edit(string id)
        {
            var category = await _context.Categories.FindAsync(id);
            var categoryViewModel = JsonConvert.DeserializeObject<CategoryViewModel>(JsonConvert.SerializeObject(category, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }));
            if (category == null)
            {
                var statusMessage = new StatusMessageModel
                {
                    Success = false,
                    Title = "Danh mục không tồn tại"
                };

                TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
                return RedirectToAction(nameof(Index));
            }
            return View(categoryViewModel);
        }

        [HttpPost]
        [Authorization(PermissionData.Update)]
        public async Task<ActionResult> Edit(string id, CategoryViewModel categoryViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    categoryViewModel.CategorySlug = AppUtilities.GenerateSlug(categoryViewModel.CategoryName);
                    var category = await _context.Categories.FindAsync(id);
                    if (category == null)
                    {
                        var statusMessage = new StatusMessageModel
                        {
                            Success = false,
                            Title = "Danh mục không tồn tại"
                        };

                        TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
                        return RedirectToAction(nameof(Index));
                    }

                    if (category.CategorySlug == categoryViewModel.CategorySlug
                    || !await _context.Categories.AnyAsync(c => c.CategorySlug == categoryViewModel.CategorySlug))
                    {
                        category.CategoryName = categoryViewModel.CategoryName;
                        category.CategorySlug = categoryViewModel.CategorySlug;
                        category.CategoryDescription = categoryViewModel.CategoryDescription;
                        if (!(await _context.Categories.AnyAsync(c => c.CategoryId == categoryViewModel.ParentCategoryId))) throw new Exception("Danh mục cha không tồn tại");
                        category.ParentCategoryId = categoryViewModel.ParentCategoryId;
                        try
                        {
                            _context.Categories.AddOrUpdate(category);
                            await _context.SaveChangesAsync();
                        }
                        catch
                        {
                            throw new Exception("Không thể chỉnh sửa danh mục");
                        }
                        var statusMessage = new StatusMessageModel
                        {
                            Success = true,
                            Title = "Chỉnh sửa danh mục thành công"
                        };

                        TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
                        return RedirectToAction(nameof(Index));
                    }
                    else throw new Exception("Danh mục đã tồn tại");
                }
            }
            catch (Exception ex)
            {
                var statusMessage = new StatusMessageModel
                {
                    Success = false,
                    Title = "Danh mục không tồn tại",
                    Message = ex.Message
                };

                TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
            }

            return View();
        }
    }
}