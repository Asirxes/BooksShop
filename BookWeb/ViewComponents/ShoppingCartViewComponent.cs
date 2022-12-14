using System.Security.Claims;
using Book.DataAccess.Repository.IRepository;
using Book.Utility;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.ViewComponents;

public class ShoppingCartViewComponent : ViewComponent
{
    private readonly IUnitOfWork _unitOfWork;

    public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        if (claim != null)
        {
            if (HttpContext.Session.GetInt32(SD.SessionCart) != null)
                return View(HttpContext.Session.GetInt32(SD.SessionCart));

            HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);
            return View(HttpContext.Session.GetInt32(SD.SessionCart));
        }

        HttpContext.Session.Clear();
        return View(0);
    }
}