using Microsoft.Extensions.Logging;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuVien.Business.Services.Implementations;

public class CategoryService
    (IUnitOfWork unitOfWork, ILogger<CategoryService> logger)
    : DataService<Category>(unitOfWork, logger), ICategoryService
{
    public async Task AddAsync(CategoryEditVm categoryEditVm)
    {
        await AddAsync(new Category()
        {
            Name = categoryEditVm.Name,
            Description = categoryEditVm.Description
        });
    }

    public async Task UpdateAsync(Guid id, CategoryEditVm categoryEditVm)
    {
        await UpdateAsync(new Category()
        {
            Id = id,
            Name = categoryEditVm.Name,
            Description = categoryEditVm.Description
        });
    }
}
