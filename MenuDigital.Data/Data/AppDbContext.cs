using MenuDigital.Common.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MenuDigital.Data.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<RestaurantUser> RestaurantUsers => Set<RestaurantUser>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Email único por restaurante (usuario)
        modelBuilder.Entity<RestaurantUser>()
            .HasIndex(x => x.Email)
            .IsUnique();

        // RestaurantUser (1) -> (N) Categories
        modelBuilder.Entity<Category>()
            .HasOne(c => c.RestaurantUser)
            .WithMany(r => r.Categories)
            .HasForeignKey(c => c.RestaurantUserId)
            .OnDelete(DeleteBehavior.Cascade);

        // RestaurantUser (1) -> (N) Products
        modelBuilder.Entity<Product>()
            .HasOne(p => p.RestaurantUser)
            .WithMany(r => r.Products)
            .HasForeignKey(p => p.RestaurantUserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Category (1) -> (N) Products (CategoryId es nullable)
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        // Evitar problemas típicos con decimal en SQLite (recomendación práctica)
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(10, 2);
    }
}
