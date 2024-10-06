using QLThuVien.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuVien.Business.Services.Interfaces;

public interface IRecommenderService
{
    Task<IEnumerable<BookVm>>  GetRecommendedBooksFromCandidates(Guid userId, int count);
    Task<IEnumerable<BookVm>> GetSimilarBookVms(Guid bookId, int count);
    Task<IEnumerable<BookVm>> GetRecommendedBooksBaselineVm(Guid userId, int count);
    Task<IEnumerable<BookVm>> GetHighestRatedBooksVm(int count);
}
