using Microsoft.Extensions.Logging;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Implementations;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using System.Linq.Expressions;

namespace QLThuVien.Business.Services.Interfaces;

public interface IBookService : IDataService<Book>
{
    Task<PaginatedResult<BookVm>> GetAsyncVm
        (
            int pageIndex = 1,
            int pageSize = 10,
            string includeProperties = "",
            Expression<Func<Book, bool>>? filter = null,
            Func<IQueryable<Book>, IOrderedQueryable<Book>>? orderBy = null
        );

    Task AddAsync(BookVm bookVm);
    Task UpdateAsync(Guid id, BookVm bookVm);
}
