using Microsoft.EntityFrameworkCore;
using QLThuVien.Business.Exceptions;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuVien.Business.Services.Implementations;

public class RecommenderService
    (IUnitOfWork unitOfWork, IBookService bookService)
    : IRecommenderService
{
    public async Task<IEnumerable<BookVm>> GetRecommendedBooks(Guid userId)
    {
        var favCategoryIds = (await unitOfWork.GetRepository<User>()
            .Get(user => user.Id == userId, null, "FavoriteCategories")
            .FirstOrDefaultAsync())
            ?.FavoriteCategories.Select(c => c.Id)
            ?? throw new NotFoundException("UserId not found");

        return (await unitOfWork.GetRepository<Book>()
            .Get(book => favCategoryIds.Contains(book.CategoryId), null, "Category,Ratings")
            .OrderBy(book => book.Ratings.Sum(rating => rating.Value))
            .Take(5)
            .ToListAsync())
            .Select(bookService.ToBookVm);
    }

    public async Task<IEnumerable<BookVm>> GetHighestRatedBooks()
    {
        return (await unitOfWork.GetRepository<Book>()
            .Get(null, null, "Category,Ratings")
            .OrderBy(book => book.Ratings.Sum(rating => rating.Value))
            .Take(5)
            .ToListAsync())
            .Select(bookService.ToBookVm);
    }
}
