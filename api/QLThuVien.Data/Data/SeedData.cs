using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Implementations;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;
using System.Data;

namespace QLThuVien.Data.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider services)
    {
        await EnsureRoles(services);
        await EnsureUsers(services);
    }

    public static async Task EnsureRoles(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<Role>>();

        await roleManager.CreateAsync(new Role()
        {
            Name = "admin",
            NormalizedName = "admin",
            Description = "Thủ thư"
        });

        await roleManager.CreateAsync(new Role()
        {
            Name = "member",
            NormalizedName = "member",
            Description = "Thành viên"
        });
    }

    public static async Task EnsureUsers(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<Role>>();

        if (await userManager.FindByEmailAsync("admin@gmail.com") == null)
        {
            await userManager.CreateAsync(new User()
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                FullName = "Thủ Thư",
                Gender = "male",
                Roles = [await roleManager.FindByNameAsync("admin") ?? throw new Exception("Roles Uninitialized")],
            }, "Admin_123");
        }
    }
}