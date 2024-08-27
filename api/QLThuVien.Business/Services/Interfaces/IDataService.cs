using System.Linq.Expressions;
using QLThuVien.Business.ViewModels;

namespace QLThuVien.Business.Services.Interfaces;

public interface IDataService<T>
    where T : class
{
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteAsync(params object?[]? keys);

    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();

    Task<PaginatedResult<T>> GetAsync(
        int pageIndex = 1,
        int pageSize = 10,
        string includeProperties = "",
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null
        );
}