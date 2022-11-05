using Book.DataAccess.Repository.IRepository;
using Book.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookWeb.Controllers;

[Area("Admin")]
public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Upsert(int? id)
    {
        Company company = new();

        if (id == null || id == 0) return View(company);

        company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);

        return View(company);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Company obj)
    {
        if (ModelState.IsValid)
        {
            if (obj.Id == 0)
            {
                TempData["success"] = "Company created successfully";

                _unitOfWork.Company.Add(obj);
            }
            else
            {
                TempData["success"] = "Company updated successfully";

                _unitOfWork.Company.Update(obj);
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
        var companyList = _unitOfWork.Company.GetAll();

        return Json(new { data = companyList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);

        if (obj == null) return Json(new { success = false, message = "Error while deleting" });

        _unitOfWork.Company.Remove(obj);

        _unitOfWork.Save();

        return Json(new { success = true, message = "Delete Successful" });
    }

    #endregion
}