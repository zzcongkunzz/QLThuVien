using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QLThuVien.Business.Models;
using QLThuVien.Data.Models;

namespace QLThuVien.Data.Data;

public class AppDbContext : IdentityDbContext<User, Role, Guid>
{
    #region DbSet

    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<FavoriteCategory> FavoriteCategories { get; set; }
    public DbSet<Borrow> Borrows { get; set; }
    public DbSet<Penalty> Penalties { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    #endregion
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public AppDbContext()
    {
    }
    
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
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-RHIBN05\SQLEXPRESS;Initial Catalog=Assignment7;Integrated Security=True;TrustServerCertificate=True;");
        }
    }
}