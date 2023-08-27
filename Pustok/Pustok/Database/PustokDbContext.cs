using Microsoft.EntityFrameworkCore;
using Pustok.Database.Models;

namespace Pustok.Database;

public class PustokDbContext : DbContext
{
    public PustokDbContext(DbContextOptions<PustokDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //FLUENT API

        modelBuilder
            .Entity<CategoryProduct>()
            .ToTable("CategoryProducts");

        modelBuilder
           .Entity<CategoryProduct>()
           .HasKey(cp => new { cp.ProductId, cp.CategoryId });

        modelBuilder
            .Entity<CategoryProduct>()
            .HasOne<Product>(ct => ct.Product)
            .WithMany(p => p.CategoryProducts)
            .HasForeignKey(ct => ct.ProductId);

        modelBuilder
            .Entity<CategoryProduct>()
            .HasOne<Category>(ct => ct.Category)
            .WithMany(p => p.CategoryProducts)
            .HasForeignKey(ct => ct.CategoryId);


        base.OnModelCreating(modelBuilder);
    }


    public DbSet<Product> Products { get; set; }
    public DbSet<CategoryProduct> CategoryProducts { get; set; }
    public DbSet<SlideBanner> SlideBanners { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Email> Emails { get; set; }
}
