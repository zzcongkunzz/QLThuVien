using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Models;

namespace QLThuVien.Business.Services.Interfaces;

public interface IRoleService : IDataService<Role>
{
    Task<IEnumerable<RoleVm>> GetAllAsyncVm();
}
