using System.Linq.Expressions;
using Book.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Book.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;

    internal DbSet<T> dbSet;

    public Repository(ApplicationDbContext db)
    {
        _db = db;

        dbSet = _db.Set<T>();
    }

    public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;

        query = query.Where(filter);

        if (includeProperties != null)
            foreach (var inclueProp in includeProperties.Split(new[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(inclueProp);

        return query.FirstOrDefault();
    }

    public IEnumerable<T> GetAll(string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;

        if (includeProperties != null)
            foreach (var inclueProp in includeProperties.Split(new[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(inclueProp);

        return query.ToList();
    }

    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
    }
}