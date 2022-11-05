using Book.DataAccess.Repository.IRepository;

namespace Book.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;

        Category = new CategoryRepository(_db);

        CoverType = new CoverTypeRepository(_db);

        Product = new ProductRepository(_db);

        Company = new CompanyRepository(_db);

        ApplicationUser = new ApplicationUserRepository(_db);

        ShoppingCart = new ShoppingCartRepository(_db);
    }

    public ICategoryRepository Category { get; }

    public ICoverTypeRepository CoverType { get; }

    public IProductRepository Product { get; }

    public ICompanyRepository Company { get; }

    public IShoppingCartRepository ShoppingCart { get; }

    public IApplicationUserRepository ApplicationUser { get; }

    public void Save()
    {
        _db.SaveChanges();
    }
}