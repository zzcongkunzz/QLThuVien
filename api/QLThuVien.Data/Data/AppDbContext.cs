using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QLThuVien.Business.Models;
using QLThuVien.Data.Models;

namespace QLThuVien.Data.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
    : IdentityDbContext<User, Role, Guid>(options)
{
    #region DbSet

    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<FavoriteCategory> FavoriteCategories { get; set; }
    public DbSet<Borrow> Borrows { get; set; }

    #endregion


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FavoriteCategory>()
            .HasKey(e => new { e.UserId, e.CategoryId });

        modelBuilder.Entity<Rating>()
            .HasKey(e => new { e.UserId, e.BookId });

        #region Relations
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<IdentityUserRole<Guid>>();

        modelBuilder.Entity<User>()
            .HasMany(u => u.BorrowingBooks)
            .WithMany(b => b.BorrowingUsers)
            .UsingEntity<Borrow>();

        modelBuilder.Entity<User>()
            .HasMany(u => u.FavoriteCategories)
            .WithMany()
            .UsingEntity<FavoriteCategory>();

        modelBuilder.Entity<User>()
            .HasMany(u => u.Ratings)
            .WithOne(r => r.User);
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Ratings)
            .WithOne(r => r.Book);
        #endregion

        SeedData.Seed(modelBuilder, this);
    }
}