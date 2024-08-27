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
            string includeProperties = "",
            Expression<Func<Book, bool>>? filter = null,
            Func<IQueryable<Book>, IOrderedQueryable<Book>>? orderBy = null
        )
    {
        var query = unitOfWork.GetRepository<Book>()
            .Get(filter, orderBy, "Category," + includeProperties)
            .Select(book => new BookVm
            {
                AuthorName = book.AuthorName,
                CategoryName = book.Category.Name,
                Description = book.Description,
                PublishDate = book.PublishDate,
                Title = book.Title
            });

        return await PaginatedResult<BookVm>.CreateAsync(query, pageIndex, pageSize);
    }

    public async Task AddAsync(BookVm bookVm)
    {
        var category = unitOfWork.GetRepository<Category>().Get(
            c => c.Name == bookVm.CategoryName, null, "Category"
            )
            .FirstOrDefault()
            ?? throw new BadRequestException("Category not found");

        await AddAsync(new Book()
        {
            AuthorName = bookVm.AuthorName,
            Category = category,
            PublishDate = bookVm.PublishDate,
            Description = bookVm.Description,
            Title = bookVm.Title
        });
    }

    public async Task UpdateAsync(Guid id, BookVm bookVm)
    {
        var category = unitOfWork.GetRepository<Category>().Get(
            c => c.Name == bookVm.CategoryName, null, "Category"
            )
            .FirstOrDefault()
            ?? throw new BadRequestException("Category not found");

        await UpdateAsync(new Book()
        {
            Id = id,
            AuthorName = bookVm.AuthorName,
            Description = bookVm.Description,
            PublishDate = bookVm.PublishDate,
            Title = bookVm.Title,
            Category = category
        });
    }
}
