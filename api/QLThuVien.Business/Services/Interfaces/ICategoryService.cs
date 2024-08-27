using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Implementations;
using QLThuVien.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuVien.Business.Services.Interfaces;

public interface ICategoryService : IDataService<Category>
{
    Task AddAsync(CategoryEditVm categoryEditVm);
    Task UpdateAsync(Guid id, CategoryEditVm categoryEditVm);
}
