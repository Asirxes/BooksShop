using Book.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Book.DataAccess;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<CoverType> CoverTypes { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<AppliactionUser> AppliactionUsers { get; set; }

    public DbSet<Company> Companies { get; set; }
}