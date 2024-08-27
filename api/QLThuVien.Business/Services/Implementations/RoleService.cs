using Microsoft.Extensions.Logging;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;

namespace QLThuVien.Business.Services.Implementations;

public class RoleService
    (
        IUnitOfWork unitOfWork,
        ILogger<RoleService> logger
    )
    : DataService<Role>(unitOfWork, logger), IRoleService
{
    public async Task<IEnumerable<RoleVm>> GetAllAsyncVm()
    {
        return (await GetAllAsync()).Select(e => new RoleVm()
        {
            Name = e.Name,
            Description = e.Description
        });
    }
}
