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
            await EnsureRatings(services);
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
                Name = "Kỳ bí"
            });
            repository.Add(new Category()
            {
                Name = "Viễn tưởng"
            });
            repository.Add(new Category()
            {
                Name = "Tâm lý"
            });
            repository.Add(new Category()
            {
                Name = "Hài hước"
            });
            repository.Add(new Category()
            {
                Name = "Khoa học"
            });
            repository.Add(new Category()
            {
                Name = "Lịch sử"
            });
            repository.Add(new Category()
            {
                Name = "Văn hóa"
            });
            repository.Add(new Category()
            {
                Name = "Chính trị"
            });
            repository.Add(new Category()
            {
                Name = "Xã hội"
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
            Category scifi = (await categoryRepository.Get(c => c.Name == "Viễn tưởng")
                .FirstOrDefaultAsync())!;
            Category mystery = (await categoryRepository.Get(c => c.Name == "Kỳ bí")
                .FirstOrDefaultAsync())!;
            Category social = (await categoryRepository.Get(c => c.Name == "Xã hội")
                .FirstOrDefaultAsync())!;
            Category psychology = (await categoryRepository.Get(c => c.Name == "Tâm lý")
                .FirstOrDefaultAsync())!;
            Category science = (await categoryRepository.Get(c => c.Name == "Khoa học")
                .FirstOrDefaultAsync())!;
            Category cultural = (await categoryRepository.Get(c => c.Name == "Văn hóa")
                .FirstOrDefaultAsync())!;
            Category politics = (await categoryRepository.Get(c => c.Name == "Chính trị")
                .FirstOrDefaultAsync())!;
            Category history = (await categoryRepository.Get(c => c.Name == "Lịch sử")
                .FirstOrDefaultAsync())!;

            bookRepository.Add(new Book()
            {
                Title = "Giải tích 1",
                AuthorName = "Nguyễn Văn A",
                Count = 50,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-3000)),
                PublisherName = "NXB Giáo Dục",
                ImageUrl = "https://nhaxuatban.vinhuni.edu.vn/data/55/upload/539/images//2017/07/bia_giai_tich_in.jpg",
                Category = educational,
                CategoryId = educational.Id,
                Description = "Sách Giải tích 1. Dùng cho các khối ngành Công nghệ, Kỹ thuật.",
            });
            bookRepository.Add(new Book()
            {
                Title = "Giải tích 2",
                AuthorName = "Nguyễn Văn A",
                Count = 20,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2000)),
                PublisherName = "NXB Giáo Dục",
                Category = educational,
                CategoryId = educational.Id,
                Description = "Sách Giải tích 2. Dùng cho các khối ngành Công nghệ, Kỹ thuật.",
                ImageUrl = "https://lib.hcmut.edu.vn/uploads/noidung/giao-trinh-giai-tich-2-0-367.jpg",
            });
            bookRepository.Add(new Book()
            {
                Title = "Giải tích 3",
                AuthorName = "Nguyễn Văn A",
                Count = 40,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1000)),
                PublisherName = "NXB Giáo Dục",
                Category = educational,
                CategoryId = educational.Id,
                Description = "Sách Giải tích 3. Dùng cho các khối ngành Công nghệ, Kỹ thuật.",
                ImageUrl = "https://images.nxbbachkhoa.vn/Picture/2022/6/1/image-20220601151312549.jpg"
            });
            bookRepository.Add(new Book()
            {
                Title = "Những điều bí ẩn trên thế giới",
                AuthorName = "Trần Văn C",
                Count = 5,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-4000)),
                PublisherName = "NXB Z",
                Category = mystery,
                CategoryId = mystery.Id,
                Description = "Tuyển tập các câu chuyện cười Việt Nam dành cho thiếu nhi.",
                ImageUrl = "https://minhkhai.com.vn/hinhlon/134311.jpg"
            });
            bookRepository.Add(new Book()
            {
                Title = "Truyện cười dân gian Việt Nam",
                AuthorName = "Trần Văn B",
                Count = 12,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1000)),
                PublisherName = "NXB Kim Đồng",
                Category = comedy,
                CategoryId = comedy.Id,
                Description = "Tuyển tập các câu chuyện cười Việt Nam dành cho thiếu nhi.",
                ImageUrl = "https://minhkhai.com.vn/hinhlon/138596.jpg"
            });
            bookRepository.Add(new Book()
            {
                Title = "All Tomorrows",
                AuthorName = "C. M. Kosemen",
                Count = 24,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-3500)),
                PublisherName = "NXB A",
                Category = scifi,
                CategoryId = scifi.Id,
                Description = "Tác phẩm nói về giả định sự tiến hoá của loài người trong tương lai trong một tỷ năm kể từ hiện tại.",
                ImageUrl = "https://cdn.shopify.com/s/files/1/0728/3615/3620/files/Qu.jpg?v=1721651114"
            });
            bookRepository.Add(new Book()
            {
                Title = "Tâm lý học",
                AuthorName = "Trần Thị C",
                Count = 30,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2500)),
                PublisherName = "NXB A",
                Category = psychology,
                CategoryId = psychology.Id,
                Description = "Tổng quan về lĩnh vực tâm lý học.",
                ImageUrl = "https://bizweb.dktcdn.net/100/180/408/products/0f1167f115d3462689fa46f6c120d3b1-fb196e72-1f32-47d3-b1f7-5e13ef8ca498.jpg?v=1615803844217"
            });
            bookRepository.Add(new Book()
            {
                Title = "Bách khoa toàn thư",
                AuthorName = "Trần Thị C",
                Count = 45,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2500)),
                PublisherName = "NXB A",
                Category = science,
                CategoryId = science.Id,
                Description = "Cung cấp tri thức tổng quan về mọi lĩnh vực.",
                ImageUrl = "https://product.hstatic.net/200000343865/product/bach-khoa-thu-bang-hinh_bia_gay-23_3d0a0ef5ed944bdfbeb5ec5dc8dad587.jpg"
            });
            bookRepository.Add(new Book()
            {
                Title = "Tư tưởng Hồ Chí Minh",
                AuthorName = "Trần Thị C",
                Count = 55,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-6000)),
                PublisherName = "NXB Sự Thật",
                Category = politics,
                CategoryId = politics.Id,
                Description = "Một số chuyên đề lý luận và thực tiễn trong Tư tưởng Hồ Chí Minh.",
                ImageUrl = "https://www.nxbctqg.org.vn/img_data/images/590078416353_a4aeee8b-6cc8-4b00-b8f7-345c16fc650b.jpg"
            });
            bookRepository.Add(new Book()
            {
                Title = "Kinh tế chính trị Mác-Lênin",
                AuthorName = "Trần Thị C",
                Count = 35,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-6500)),
                PublisherName = "NXB Sự Thật",
                Category = politics,
                CategoryId = politics.Id,
                Description = "Giáo trình Kinh tế chính trị Mác-Lênin.",
                ImageUrl = "https://stbook.vn/static/covers/CP111BK120211115135135/cover.clsbi"
            });
            bookRepository.Add(new Book()
            {
                Title = "Triết học Mác-Lênin",
                AuthorName = "Trần Thị C",
                Count = 25,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-6800)),
                PublisherName = "NXB Sự Thật",
                Category = politics,
                CategoryId = politics.Id,
                Description = "Giáo trình Triết học Mác-Lênin.",
                ImageUrl = "https://images.sachquocgia.vn/Picture/2024/3/21/image-20240321142038119.jpg"
            });
            bookRepository.Add(new Book()
            {
                Title = "Cẩm nang du lịch Việt Nam",
                AuthorName = "Trần Thị Q",
                Count = 55,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5000)),
                PublisherName = "NXB D",
                Category = cultural,
                CategoryId = cultural.Id,
                Description = "Cẩm nang hướng dẫn du lịch Việt Nam.",
                ImageUrl = "https://bizweb.dktcdn.net/100/116/097/files/cn-du-lich.jpg?v=1541078767830"
            });
            bookRepository.Add(new Book()
            {
                Title = "Đại Việt Sử ký Toàn Thư",
                AuthorName = "Nhiều tác giả",
                Count = 55,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5000)),
                PublisherName = "NXB D",
                Category = history,
                CategoryId = history.Id,
                Description = "Bộ quốc sử danh tiếng, một di sản quý báu của dân tộc Việt Nam nghìn năm văn hiến. Đó là bộ sử cái, có giá trị nhiều mặt, gắn liền với tên tuổi các nhà sử học nổi tiếng như Lê Văn Hưu, Phan Phu Tiên, Ngô Sĩ Liên, Phạm Công Trứ, Lê Hy.",
                ImageUrl = "http://isach.info/images/story/cover/dai_viet_su_ky_toan_thu.jpg"
            });
            bookRepository.Add(new Book()
            {
                Title = "Thế giới động vật",
                AuthorName = "Nhiều tác giả",
                Count = 12,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-3500)),
                PublisherName = "NXB S",
                Category = science,
                CategoryId = science.Id,
                Description = "Cung cấp thông tin bổ ích về các loài vật.",
                ImageUrl = "https://img.lazcdn.com/g/p/d439168d9fbcc84cf36988306fef9f8c.jpg_960x960q80.jpg_.webp"
            });
            bookRepository.Add(new Book()
            {
                Title = "Xu hướng dân số Việt Nam",
                AuthorName = "Nhiều tác giả",
                Count = 28,
                PublishDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-100)),
                PublisherName = "NXB S",
                Category = social,
                CategoryId = social.Id,
                Description = "Cung cấp thông tin về xu hướng dân số Việt Nam.",
                ImageUrl = "https://www.google.com/url?sa=i&url=http%3A%2F%2Fcantholib.org.vn%2Fchinh-tri-xa-hoi%2Fnhung-xu-huong-bien-doi-dan-so-o-viet-nam-sach-chuyen-khao-nguyen-dinh-cu-h-nong-nghiep-2007-395tr-21cm-441.html&psig=AOvVaw1IpqJjXtU0ENA2lWnbqQbE&ust=1728288330200000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCOC-7PKl-YgDFQAAAAAdAAAAABAE"
            });
            await unitOfWork.SaveChangesAsync();
        }
    }

    public static async Task EnsureRatings(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<User>>();

        var unitOfWork = services.GetRequiredService<IUnitOfWork>();

        var admin = await userManager.FindByEmailAsync("admin@gmail.com") 
            ?? throw new InvalidOperationException();
            
        if (!(await unitOfWork.GetRepository<Rating>().GetQuery().AnyAsync()))
        {
            var bookIds = await unitOfWork.GetRepository<Book>().GetQuery().Take(12)
                .Select(b => b.Id).ToArrayAsync();
            var rand = new Random();
            foreach (var bookId in bookIds)
            {
                unitOfWork.GetRepository<Rating>().Add(new Rating()
                {
                    UserId = admin.Id,
                    BookId = bookId,
                    Value = 3 + rand.NextDouble() * 2
                });
            }
            await unitOfWork.SaveChangesAsync();
        }
    }
}