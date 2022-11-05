﻿using Book.DataAccess.Repository.IRepository;
using Book.Models;

namespace Book.DataAccess.Repository;

public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
{
    private readonly ApplicationDbContext _db;

    public ShoppingCartRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public int IncrementCount(ShoppingCart shoppingCart, int count)
    {
        shoppingCart.Count += count;

        return shoppingCart.Count;
    }

    public int DecrementCount(ShoppingCart shoppingCart, int count)
    {
        shoppingCart.Count -= count;

        return shoppingCart.Count;
    }
}