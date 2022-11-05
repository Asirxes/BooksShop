﻿using System.Security.Claims;
using Book.DataAccess.Repository.IRepository;
using Book.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookWeb.Controllers;

[Authorize]
[Area("Customer")]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CartController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ShoppingCartVM ShoppingCartVM { get; set; }

    public int OrderTotal { get; set; }

    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;

        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        ShoppingCartVM = new ShoppingCartVM
        {
            ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, "Product")
        };

        foreach (var cart in ShoppingCartVM.ListCart)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50,
                cart.Product.Price100);
            ShoppingCartVM.CartTotal += cart.Price * cart.Count;
        }

        return View(ShoppingCartVM);
    }

    public IActionResult Summary()
    {
        return View();
    }

    public IActionResult Plus(int cartId)
    {
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);

        _unitOfWork.ShoppingCart.IncrementCount(cart, 1);

        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Minus(int cartId)
    {
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);

        if (cart.Count <= 1)
            _unitOfWork.ShoppingCart.Remove(cart);
        else
            _unitOfWork.ShoppingCart.DecrementCount(cart, 1);

        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove(int cartId)
    {
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);

        _unitOfWork.ShoppingCart.Remove(cart);

        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    private double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
    {
        if (quantity <= 50)
        {
            return price;
        }

        if (quantity <= 100) return price50;

        return price100;
    }
}