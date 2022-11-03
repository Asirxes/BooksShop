﻿using Book.DataAccess.Repository.IRepository;

namespace Book.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;

        Category = new CategoryRepository(db);

        CoverType = new CoverTypeRepository(db);

        Product = new ProductRepository(db);
    }

    public ICategoryRepository Category { get; }

    public ICoverTypeRepository CoverType { get; }

    public IProductRepository Product { get; }

    public void Save()
    {
        _db.SaveChanges();
    }
}