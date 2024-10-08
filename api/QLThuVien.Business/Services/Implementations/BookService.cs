﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QLThuVien.Business.Exceptions;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using System.Linq.Expressions;

namespace QLThuVien.Business.Services.Implementations;

public class BookService
    (
        IUnitOfWork unitOfWork,
        ILogger<BookService> logger
    ) : DataService<Book>(unitOfWork, logger), IBookService
{
    public async Task<IEnumerable<BookVm>> GetAll()
    {
        return (await unitOfWork.GetRepository<Book>()
                    .GetQuery().Include(book => book.Category).Include(book => book.Ratings)
                    .ToListAsync()).Select(book => ToBookVm(book));
    }

    public async Task<PaginatedResult<BookVm>> QueryAsyncVm
        (
            int pageIndex = 1,
            int pageSize = 10,
            Expression<Func<Book, bool>>? filter = null,
            Func<IQueryable<Book>, IOrderedQueryable<Book>>? orderBy = null
        )
    {
        var query = unitOfWork.GetRepository<Book>()
            .Get(filter, orderBy, "Category,Ratings");

        var pgBook = await PaginatedResult<Book>.CreateAsync(query, pageIndex, pageSize);

        return new PaginatedResult<BookVm>(
            pgBook.Items.Select(ToBookVm).ToList(), 
            await query.CountAsync(), pageIndex, pageSize);
    }

    public async Task<BookVm> GetByIdAsyncVm(Guid id)
    {
        var book = await unitOfWork.GetRepository<Book>()
            .Get(book => book.Id == id, null, "Category,Ratings")
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException("Id not found");
        return ToBookVm(book);
    }

    public async Task AddAsync(BookEditVm bookEditVm)
    {
        var category = unitOfWork.GetRepository<Category>().Get(c => 
                c.Name == bookEditVm.CategoryName)
            .FirstOrDefault()
            ?? throw new BadRequestException("Category not found");

        await AddAsync(new Book()
        {
            AuthorName = bookEditVm.AuthorName,
            Category = category,
            PublisherName = bookEditVm.PublisherName,
            PublishDate = bookEditVm.PublishDate,
            Description = bookEditVm.Description,
            Count = bookEditVm.Count,
            Title = bookEditVm.Title
        });
    }

    public async Task UpdateAsync(Guid id, BookEditVm bookEditVm)
    {
        var category = unitOfWork.GetRepository<Category>().Get(
            c => c.Name == bookEditVm.CategoryName
            )
            .FirstOrDefault()
            ?? throw new BadRequestException("Category not found");

        await UpdateAsync(new Book()
        {
            Id = id,
            AuthorName = bookEditVm.AuthorName,
            Description = bookEditVm.Description,
            PublisherName = bookEditVm.PublisherName,
            PublishDate = bookEditVm.PublishDate,
            Title = bookEditVm.Title,
            Count = bookEditVm.Count,
            Category = category
        });
    }

    public async Task<int> GetRemainingCountAsync(Guid id)
    {
        var book = (await GetByIdAsync(id)) ?? throw new NotFoundException("id not found");
        return book.Count - await unitOfWork.GetRepository<Borrow>()
            .Get(borrow => borrow.BookId == id 
                && borrow.ActualReturnTime == null).SumAsync(borrow => borrow.Count);
    }

    public BookVm ToBookVm(Book book)
    {
        double? avgRating = (
            book.Ratings != null && book.Ratings.Any()
            ? book.Ratings.Average(rating => rating.Value)
            : null
            );

        return new BookVm()
        {
            Id = book.Id,
            Title = book.Title,
            CategoryName = book.Category.Name,
            Description = book.Description,
            PublisherName = book.PublisherName,
            PublishDate = book.PublishDate,
            AuthorName = book.AuthorName,
            Count = book.Count,
            ImageUrl = book.ImageUrl ?? "/images/book.png",
            AverageRating = avgRating
        };
    }
    public async Task<double?> GiveRating(RatingVm ratingVm)
    {
        var repo = unitOfWork.GetRepository<Rating>();
        var old = await repo.GetByIdAsync(ratingVm.UserId, ratingVm.BookId);

        if (old != null)
        {
            old.Value = ratingVm.Value;
            repo.Update(old);
        }
        else
        {
            repo.Add(new Rating()
            {
                UserId = ratingVm.UserId,
                BookId = ratingVm.BookId,
                Value = ratingVm.Value
            });
        }

        await unitOfWork.SaveChangesAsync();

        var book = await unitOfWork.GetRepository<Book>()
            .Get(book => book.Id == ratingVm.BookId, null, "Ratings")
            .FirstOrDefaultAsync()
            ?? throw new ConflictException("Book has been deleted");

        return book.Ratings != null && book.Ratings.Any()
            ? book.Ratings.Average(book => book.Value)
            : null;
    }
}
