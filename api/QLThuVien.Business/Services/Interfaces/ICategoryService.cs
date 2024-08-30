using QLThuVien.Business.Models;
using QLThuVien.Business.ViewModels;

namespace QLThuVien.Business.Services.Interfaces;

public interface ICategoryService : IDataService<Category>
{
    Task AddAsync(CategoryEditVm categoryEditVm);
    Task UpdateAsync(Guid id, CategoryEditVm categoryEditVm);
}
