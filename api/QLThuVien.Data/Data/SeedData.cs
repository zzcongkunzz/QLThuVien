using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QLThuVien.Business.Models;
using QLThuVien.Data.Models;

namespace QLThuVien.Data.Data;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder, AppDbContext dbContext)
    {
        var adminRole = new Role()
        {
            Id = Guid.NewGuid(),
            Name = "admin",
            NormalizedName = "admin",
            Description = "Thủ thư"
        };
        var memberRole = new Role()
        {
            Id = Guid.NewGuid(),
            Name = "member",
            NormalizedName = "member",
            Description = "Thành viên"
        };
        modelBuilder.Entity<Role>()
            .HasData(adminRole);
        modelBuilder.Entity<Role>()
            .HasData(memberRole);

        var admin = new User()
        {
            Id = Guid.NewGuid(),
            Email = "admin@gmail.com",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
            FullName = "admin 123",
            Gender = "male"
        };
        modelBuilder.Entity<User>()
            .HasData(admin);

        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .HasData(new IdentityUserRole<Guid>()
            {
                UserId = admin.Id,
                RoleId = adminRole.Id
            });

        var member1 = new User()
        {
            Id = Guid.NewGuid(),
            Email = "member1@gmail.com",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
            FullName = "Member1 Name",
            Gender = "female"
        };
        modelBuilder.Entity<User>()
            .HasData(member1);
        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .HasData(new IdentityUserRole<Guid>()
            {
                UserId = member1.Id,
                RoleId = memberRole.Id
            });
    }
}