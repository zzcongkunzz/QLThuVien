using QLThuVien.Business.Models;
using QLThuVien.Business.ViewModels;
using System.Linq.Expressions;

namespace QLThuVien.Business.Services.Interfaces;

public interface IBookService : IDataService<Book>
{
    Task<IEnumerable<BookVm>> GetAll();
    Task<PaginatedResult<BookVm>> QueryAsyncVm
        (
            int pageIndex = 1,
            int pageSize = 10,
            Expression<Func<Book, bool>>? filter = null,
            Func<IQueryable<Book>, IOrderedQueryable<Book>>? orderBy = null
        );
    Task<BookVm> GetByIdAsyncVm(Guid id);
    Task AddAsync(BookEditVm bookEditVm);
    Task UpdateAsync(Guid id, BookEditVm bookVm);
    BookVm ToBookVm(Book book);
    // trả về avg rating mới của book
    Task<float?> GiveRating(RatingVm ratingVm);
}
