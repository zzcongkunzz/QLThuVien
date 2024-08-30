using QLThuVien.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuVien.Business.Services.Interfaces;

public interface IRecommenderService
{
    Task<IEnumerable<BookVm>> GetRecommendedBooks(Guid userId);
    Task<IEnumerable<BookVm>> GetHighestRatedBooks();
}
