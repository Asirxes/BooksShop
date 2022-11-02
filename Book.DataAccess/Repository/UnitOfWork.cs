using Book.DataAccess.Repository.IRepository;

namespace Book.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext _db;
    
    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Category = new CategoryRepository(db);
    }
    
    public ICategoryRepository Category { get; private set; }
    
    public void Save()
    {
        _db.SaveChanges();
    }
}