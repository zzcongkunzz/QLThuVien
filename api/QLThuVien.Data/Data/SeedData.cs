using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QLThuVien.Business.Models;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;
using System;

namespace QLThuVien.Data.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider services)
    {
        var environment = services.GetRequiredService<IHostEnvironment>();

        await EnsureRoles(services);
        await EnsureUsers(services);

        if (environment.IsDevelopment())
        {
            await EnsureCategories(services);
            await EnsureBooks(services);
        }
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

    public static async Task EnsureCategories(IServiceProvider services)
    {
        var unitOfWork = services.GetRequiredService<IUnitOfWork>();
        var repository = unitOfWork.GetRepository<Category>();

        if (await repository.GetQuery().AnyAsync() == false)
        {
            repository.Add(new Category()
            {
                Name = "Giáo dục"
            });
            repository.Add(new Category()
            {
                Name = "Hành động"
            });
            repository.Add(new Category()
            {
                Name = "Viễn tưởng"
            });
            repository.Add(new Category()
            {
                Name = "Lãng mạn"
            });
            repository.Add(new Category()
            {
                Name = "Tâm lý"
            });
            repository.Add(new Category()
            {
                Name = "Hài hước"
            });
            await unitOfWork.SaveChangesAsync();
        }
    }

    public static async Task EnsureBooks(IServiceProvider services)
    {
        var unitOfWork = services.GetRequiredService<IUnitOfWork>();
        var bookRepository = unitOfWork.GetRepository<Book>();
        var categoryRepository = unitOfWork.GetRepository<Category>();

        if (await bookRepository.GetQuery().AnyAsync() == false)
        {
            Category educational = (await categoryRepository.Get(c => c.Name == "Giáo dục")
                .FirstOrDefaultAsync())!;
            Category comedy = (await categoryRepository.Get(c => c.Name == "Hài hước")
                .FirstOrDefaultAsync())!;

            bookRepository.Add(new Book()
            {
                Title = "Giải tích 1",
                AuthorName = "Nguyễn Văn A",
                Count = 50,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-3000)),
                PublisherName = "NXB Giáo Dục",
                Category = educational,
                CategoryId = educational.Id,
                Description = "Sách Giải tích 1. Dùng cho các khối ngành Công nghệ, Kỹ thuật.",
            });
            bookRepository.Add(new Book()
            {
                Title = "Giải tích 2",
                AuthorName = "Nguyễn Văn A",
                Count = 30,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2000)),
                PublisherName = "NXB Giáo Dục",
                Category = educational,
                CategoryId = educational.Id,
                Description = "Sách Giải tích 2. Dùng cho các khối ngành Công nghệ, Kỹ thuật.",
            });
            bookRepository.Add(new Book()
            {
                Title = "Truyện cười thiếu nhi",
                AuthorName = "Trần Văn B",
                Count = 12,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1000)),
                PublisherName = "NXB Kim Đồng",
                Category = comedy,
                CategoryId = comedy.Id,
                Description = "Tuyển tập các câu chuyện cười Việt Nam dành cho thiếu nhi.",
            });
            await unitOfWork.SaveChangesAsync();
        }
    }
}