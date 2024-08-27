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
    //public DbSet<Borrow> Borrows { get; set; }
    //public DbSet<Penalty> Penalties { get; set; }

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
        #endregion

        SeedData.Seed(modelBuilder, this);
    }
}