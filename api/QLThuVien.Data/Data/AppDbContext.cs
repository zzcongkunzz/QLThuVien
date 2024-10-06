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
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    //public DbSet<RawMLModel> MLModels { get; set; }
    //public DbSet<RawFeatureExtractor> RawFeatureExtractors { get; set; }
    //public DbSet<CategoryIndex> CategoryIndices { get; set; }
    #endregion

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
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
            .HasMany(u => u.Borrows)
            .WithOne(b => b.User);
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Borrows)
            .WithOne(b => b.Book);

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

        //modelBuilder.Entity<RawMLModel>()
        //    .HasOne(m => m.RawFeatureExtractor)
        //    .WithOne(e => e.RawMLModel);
        //modelBuilder.Entity<RawFeatureExtractor>()
        //    .HasMany(e => e.CategoryIndices)
            //.WithOne();
        #endregion
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}