using Exagochi.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exagochi.Api.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Entities.Exagochi> Exagochis { get; set; } = null!;
    public DbSet<Challenge> Challenges { get; set; } = null!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}