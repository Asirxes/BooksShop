using Book.DataAccess.Repository.IRepository;
using Book.Models;

namespace Book.DataAccess.Repository;

public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
{
    private readonly ApplicationDbContext _db;

    public CoverTypeRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(CoverType obj)
    {
        _db.CoverTypes.Update(obj);
    }
}