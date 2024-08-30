using QLThuVien.Business.Models;
using QLThuVien.Business.ViewModels;
using System.Linq.Expressions;

namespace QLThuVien.Business.Services.Interfaces;

public interface IBorrowService : IDataService<Borrow>
{
    Task<IEnumerable<BorrowVm>> GetAllVm();
    Task<PaginatedResult<BorrowVm>> GetAsyncVm
        (
            int pageIndex = 1,
            int pageSize = 10,
            Expression<Func<Borrow, bool>>? filter = null,
            Func<IQueryable<Borrow>, IOrderedQueryable<Borrow>>? orderBy = null
        );
    Task<BorrowVm> GetByIdAsyncVm(Guid id);
    Task AddAsync(BorrowEditVm borrowEditVm);
    Task UpdateAsync(Guid id, BorrowEditVm borrowEditVm);
    Task<DateTime> ReturnBorrow(Guid id);
    Task UndoReturnBorrow(Guid id);
}