using Microsoft.EntityFrameworkCore;
using TheBeans.Core.Entities;

namespace TheBeans.Infrastructure.Data
{
    public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<CoffeeBean> CoffeeBeans { get; set; }
    public DbSet<BeanOfTheDay> BeansOfTheDay { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        // Configure entity relationships, indexes, etc.
        modelBuilder.Entity<BeanOfTheDay>()
            .HasIndex(b => b.SelectedDate)
            .IsUnique();
    }
}
}