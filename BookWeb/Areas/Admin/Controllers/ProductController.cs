using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Book.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookWeb.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;

        _hostEnvironment = hostEnvironment;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Upsert(int? id)
    {
        ProductVM productVm = new()
        {
            Product = new Product(),
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            })
        };

        if (id == null || id == 0)
            // ViewBag.CategoryList = CategoryList;
            // ViewData["CoverTypeList"] = CoverTypeList;
            return View(productVm);

        productVm.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

        return View(productVm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            var wwwRootPath = _hostEnvironment.WebRootPath;

            if (file != null)
            {
                var fileName = Guid.NewGuid().ToString();

                var uploads = Path.Combine(wwwRootPath, @"images\products");

                var extension = Path.GetExtension(file.FileName);

                if (obj.Product.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                }

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }

                obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
            }

            if (obj.Product.Id == 0)
            {
                TempData["success"] = "Product created successfully";
                _unitOfWork.Product.Add(obj.Product);
            }
            else
            {
                TempData["success"] = "Product updated successfully";
                _unitOfWork.Product.Update(obj.Product);
            }

            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        return View(obj);
    }

    #region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Product.GetAll("Category,CoverType");

        return Json(new { data = productList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
        if (obj == null) return Json(new { success = false, message = "Error while deleting" });

        var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));

        if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);

        _unitOfWork.Product.Remove(obj);

        _unitOfWork.Save();

        return Json(new { success = true, message = "Delete Successful" });

        return RedirectToAction("Index");
    }

    #endregion
}