using Microsoft.Extensions.Logging;
using QLThuVien.Business.Exceptions;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;
using System.Linq.Expressions;

namespace QLThuVien.Business.Services.Implementations;

public class BookService
    (
        IUnitOfWork unitOfWork,
        ILogger<BookService> logger
    ) : DataService<Book>(unitOfWork, logger), IBookService
{
    public async Task<PaginatedResult<BookVm>> GetAsyncVm
        (
            int pageIndex = 1,
            int pageSize = 10,
            string includeProperties = "Category",
            Expression<Func<Book, bool>>? filter = null,
            Func<IQueryable<Book>, IOrderedQueryable<Book>>? orderBy = null
        )
    {
        var query = unitOfWork.GetRepository<Book>()
            .Get(filter, orderBy, includeProperties)
            .Select(book => new BookVm
            {
                Id = book.Id,
                AuthorName = book.AuthorName,
                CategoryName = book.Category.Name,
                Description = book.Description,
                PublishDate = book.PublishDate,
                Title = book.Title
            });

        return await PaginatedResult<BookVm>.CreateAsync(query, pageIndex, pageSize);
    }

    public async Task<BookVm> GetByIdAsyncVm(Guid id)
    {
        var book = await GetByIdAsync(id);
        return new BookVm()
        {
            Id = book.Id,
            Title = book.Title,
            CategoryName = book.Category.Name,
            Description = book.Description,
            PublishDate = book.PublishDate,
            AuthorName = book.AuthorName,
        };
    }

    public async Task AddAsync(BookEditVm bookEditVm)
    {
        var category = unitOfWork.GetRepository<Category>().Get(c => 
                c.Name == bookEditVm.CategoryName)
            .FirstOrDefault()
            ?? throw new BadRequestException("Category not found");

        await AddAsync(new Book()
        {
            AuthorName = bookEditVm.AuthorName,
            Category = category,
            PublishDate = bookEditVm.PublishDate,
            Description = bookEditVm.Description,
            Title = bookEditVm.Title
        });
    }

    public async Task UpdateAsync(Guid id, BookEditVm bookEditVm)
    {
        var category = unitOfWork.GetRepository<Category>().Get(
            c => c.Name == bookEditVm.CategoryName
            )
            .FirstOrDefault()
            ?? throw new BadRequestException("Category not found");

        await UpdateAsync(new Book()
        {
            Id = id,
            AuthorName = bookEditVm.AuthorName,
            Description = bookEditVm.Description,
            PublishDate = bookEditVm.PublishDate,
            Title = bookEditVm.Title,
            Category = category
        });
    }
}
