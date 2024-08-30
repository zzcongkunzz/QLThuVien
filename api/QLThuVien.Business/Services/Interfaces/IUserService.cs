using QLThuVien.Business.Models;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Models;
using System.Linq.Expressions;

namespace QLThuVien.Business.Services.Interfaces;

public interface IUserService : IDataService<User>
{
    Task<IEnumerable<UserVm>> GetAllAsyncVms();
    Task CreateAsync(UserCreateVm userCreateVm);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Guid id, UserEditVm userEditVm);
    Task ChangePasswordAsync(Guid id, string currentPassword, string newPassword);
    Task<UserVm> GetByIdAsyncVm(Guid id);
    Task<PaginatedResult<UserVm>> GetAsyncVm(
        int pageIndex = 1,
        int pageSize = 10,
        string includeProperties = "",
        Expression<Func<User, bool>>? filter = null,
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null
        );
    Task AddFavoriteCategoryAsync(Guid userId, Guid categoryId);
    Task DeleteFavoriteCategoryAsync(Guid userId, Guid categoryId);
    Task<IEnumerable<Category>> GetFavoriteCategoriesAsync(Guid userId);
}
