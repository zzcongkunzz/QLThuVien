using Microsoft.Extensions.Logging;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Interfaces;
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
}
