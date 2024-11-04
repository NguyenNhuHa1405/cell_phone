using CellPhone.Areas.Admin.Models.Product;
using CellPhone.Models;
using CellPhone.Utilities;
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
    public class ProductController : Controller
    {
        private readonly CellPhoneContext _context = new CellPhoneContext();
        private readonly StatusMessageModel _statusMessage = new StatusMessageModel();

        public async Task<ActionResult> Index()
        {
            var products = await _context.Products.Include("Category").ToListAsync();
            ViewBag.Products = products;
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductViewModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!(await _context.Categories.AnyAsync(c => c.CategoryId == product.CategoryId))) throw new Exception("Danh mục không tồn tại");
                    var newProduct = JsonConvert.DeserializeObject<Product>(JsonConvert.SerializeObject(product));
                    newProduct.ProductId = Guid.NewGuid().ToString();
                    newProduct.ProductSlug = AppUtilities.GenerateSlug(newProduct.ProductName);
                    if (await _context.Products.AnyAsync(p => p.ProductSlug == newProduct.ProductSlug)) throw new Exception("Sản phẩm đã tồn tại");
                    if (!await _context.Categories.AnyAsync(c => c.CategoryId == newProduct.CategoryId)) throw new Exception("Danh mục không tồn tại");
                    try
                    {
                        _context.Products.Add(newProduct);
                        await _context.SaveChangesAsync();
                    }
                    catch
                    {
                        throw new Exception("Không thể thêm sản phẩm");
                    }

                    var statusMessage = new StatusMessageModel
                    {
                        Success = true,
                        Title = "Thêm sản phẩm thành công"
                    };

                    TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
                    return RedirectToAction(nameof(Create));
                }
            }
            catch (Exception ex)
            {
                var statusMessage = new StatusMessageModel
                {
                    Success = false,
                    Title = "Thêm sản phẩm không thành công",
                    Message = ex.Message
                };

                TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id) ?? throw new Exception("Sản phẩm không tồn tại");

                try
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    throw new Exception("Xoá sản phẩm thất bại");
                }

                var statusMessage = new StatusMessageModel
                {
                    Success = true,
                    Title = "Xoá sản phẩm thành công"
                };

                TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
            }
            catch (Exception ex)
            {
                var statusMessage = new StatusMessageModel
                {
                    Success = false,
                    Title = "Xoá sản phẩm không thành công",
                    Message = ex.Message
                };

                TempData.Add("StatusMessage", JsonConvert.SerializeObject(statusMessage));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id) ?? throw new Exception("Sản phẩm không tồn tại");
                var productViewModel = JsonConvert.DeserializeObject<ProductViewModel>(JsonConvert.SerializeObject(product, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                }));
                return View(productViewModel);
            }catch (Exception ex)
            {
                _statusMessage.Success = false;
                _statusMessage.Title = "Chỉnh sửa sản phẩm không thành công";
                _statusMessage.Message = ex.Message;
            }

            TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, ProductViewModel productViewModel)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var product = await _context.Products.FindAsync(id) ?? throw new Exception("Sản phẩm không tồn tại");
                    if (!(await _context.Categories.AnyAsync(c => c.CategoryId == productViewModel.CategoryId))) throw new Exception("Danh mục không tồn tại");
                    productViewModel.ProductSlug = AppUtilities.GenerateSlug(productViewModel.ProductName);
                    if (await _context.Products.AnyAsync(p => p.ProductSlug == productViewModel.ProductSlug && p.ProductSlug != product.ProductSlug))
                        throw new Exception("Sản phẩm đã tồn tại");

                    product.ProductSlug = productViewModel.ProductSlug;
                    product.ProductName = productViewModel.ProductName;
                    product.ProductDescription = productViewModel.ProductDescription;
                    product.ProductPrice = productViewModel.ProductPrice;
                    product.CategoryId = productViewModel.CategoryId;
                    try
                    {
                        _context.Products.AddOrUpdate(product);
                        await _context.SaveChangesAsync();
                    }catch { throw new Exception("Không thể chỉnh sửa"); }
                    _statusMessage.Success = true;
                    _statusMessage.Title = "Chỉnh sửa sản phẩm thành công";
                    TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _statusMessage.Success = false;
                _statusMessage.Title = "Chỉnh sửa sản phẩm không thành công";
                _statusMessage.Message = ex.Message;
            }

            TempData.Add("StatusMessage", JsonConvert.SerializeObject(_statusMessage));
            return View();
        }
    }
}