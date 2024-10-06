using Microsoft.EntityFrameworkCore;
using QLThuVien.Business.Exceptions;
using QLThuVien.Business.ML;
using QLThuVien.Business.MLModules;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;
using System.Collections.Immutable;
using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;

namespace QLThuVien.Business.Services.Implementations;

public class RecommenderService(
    IUnitOfWork unitOfWork, 
    IBookService bookService,
    ModelManager modelManager)
    : IRecommenderService
{
    public async Task<IEnumerable<BookVm>> GetRecommendedBooksFromCandidates(Guid userId, int count)
    {
        if (modelManager.IsInitialized == false)
            modelManager.LoadModelFromDisk();

        // retrieval
        var candidates = (await RetrieveCandidateBooks(userId, count)).ToArray();

        // ranking
        using var scope = NewDisposeScope();

        var userTensor = modelManager.UserScaler!.Scale_(
            (modelManager.FeatureExtractor!.GetTensor(
                (await unitOfWork.GetRepository<User>()
                    .Get(u => u.Id == userId, null, FeatureExtractor.UserIncludeProperties)
                    .FirstOrDefaultAsync()) ?? throw new NotFoundException("Id not found"))));

        var ratings = candidates
            .Select(b => modelManager.Module!
                .predict(userTensor,
                    modelManager.BookScaler!.Scale_(modelManager.FeatureExtractor!.GetTensor(b))))
            .ToArray();

        Array.Sort(ratings, candidates);

        return candidates.TakeLast(count).Reverse().Select(bookService.ToBookVm);
    }

    public async Task<IEnumerable<BookVm>> GetSimilarBookVms(Guid bookId, int count)
    {
        return (await GetSimilarBooks(bookId, count)).Select(bookService.ToBookVm);
    }


    public async Task<IEnumerable<BookVm>> GetRecommendedBooksBaselineVm(Guid userId, int count)
    {
        return (await GetRecommendedBooksBaseline(userId, count)).Select(bookService.ToBookVm);
    }

    public async Task<IEnumerable<BookVm>> GetHighestRatedBooksVm(int count)
    {
        return (await GetHighestRatedBooks(count))
            .Select(bookService.ToBookVm);
    }


    private async Task<IEnumerable<Book>> RetrieveCandidateBooks(Guid userId, int count)
    {
        // initial candidates = 2 * baseline + higest_rated
        var candidateBooks = (await GetRecommendedBooksBaseline(
            userId, count * 2, FeatureExtractor.BookIncludeProperties));
        candidateBooks = candidateBooks.Concat(
            await GetHighestRatedBooks(count, FeatureExtractor.BookIncludeProperties));

        // foreach borrowed books, find count/2 most similar books
        var borrowedBookIds = (await GetBookIdsBorrowedByUser(userId)).ToHashSet();
        foreach (var bookId in borrowedBookIds)
            candidateBooks = candidateBooks.Concat(await GetSimilarBooks(bookId, count/2));

        // remove duplicates + borrowed books
        return candidateBooks
            .Where(b => !borrowedBookIds.Contains(b.Id)).DistinctBy(b => b.Id);
    }


    
    private async Task<IEnumerable<Book>> GetSimilarBooks(Guid bookId, int count)
    {
        if (!modelManager.IsInitialized)
            modelManager.LoadModelFromDisk();

        using var scope = NewDisposeScope();

        var curBookTensor = modelManager.BookScaler!.Scale_(
            modelManager.FeatureExtractor!.GetTensor
                (await unitOfWork.GetRepository<Book>()
                .Get(b => b.Id == bookId, null, FeatureExtractor.BookIncludeProperties)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Id not found"))
            ).reshape(1, -1);

        var otherBooks = await unitOfWork.GetRepository<Book>()
            .Get(b => b.Id != bookId, null, FeatureExtractor.BookIncludeProperties)
            .ToArrayAsync();

        var otherBookTensors = concat(otherBooks
            .Select(b => modelManager.BookScaler!.Scale_(
                modelManager.FeatureExtractor!.GetTensor(b)).reshape(1, -1))
            .ToArray());

        var otherEmbeddings = modelManager.Module!.GetItemEmbedding(otherBookTensors);
        var curEmbedding = modelManager.Module!.GetItemEmbedding(curBookTensor);

        // euclidean distance
        var distances = otherEmbeddings.sub_(curEmbedding)
            .square().sum(1).flatten().data<double>().ToArray();

        // cosine distance
        //var dots = otherEmbeddings.mul(curEmbedding).sum(1);
        //var otherNorm = otherEmbeddings.square().sum(1);
        //var curNorm = curEmbedding.square().sum();
        //var distances = dots.div_(otherNorm.mul_(curNorm).sqrt_()).data<double>().ToArray();

        Array.Sort(distances, otherBooks);

        return otherBooks.Take(count);
    }


    public async Task<IEnumerable<Book>> GetRecommendedBooksBaseline(
        Guid userId, 
        int count,
        string includeProperties = "Category,Ratings")
    {
        var favCategoryIds = (await unitOfWork.GetRepository<User>()
            .Get(user => user.Id == userId, null, "FavoriteCategories")
            .FirstOrDefaultAsync())
            ?.FavoriteCategories.Select(c => c.Id)
            ?? throw new NotFoundException("UserId not found");

        var borrowedBookIds = await GetBookIdsBorrowedByUser(userId);

        var res = await unitOfWork.GetRepository<Book>()
            .Get(book =>
                favCategoryIds.Contains(book.CategoryId) && !borrowedBookIds.Contains(book.CategoryId)
                , null, includeProperties)
            .OrderByDescending(book => book.Ratings.Sum(rating => rating.Value))
            .Take(count)
            .ToListAsync();

        var ids = res.Select(b => b.Id);

        if (res.Count() < count)
            res = res.Concat(await unitOfWork.GetRepository<Book>()
            .Get(b => !ids.Contains(b.Id) && !borrowedBookIds.Contains(b.CategoryId)
                , null, includeProperties)
            .OrderByDescending(book => book.Ratings.Sum(rating => rating.Value))
            .Take(count - res.Count())
            .ToListAsync()).ToList();

        return res;
    }


    public async Task<IEnumerable<Book>> GetHighestRatedBooks(
        int count, string includeProperties = "Category,Ratings")
    {
        return await unitOfWork.GetRepository<Book>()
            .Get(null, null, includeProperties)
            .OrderByDescending(book => book.Ratings.Sum(rating => rating.Value))
            .Take(count)
            .ToListAsync();
    }


    private async Task<IEnumerable<Guid>> GetBookIdsBorrowedByUser(Guid userId)
    {
        return await unitOfWork.GetRepository<User>().GetQuery()
            .Where(u => u.Id == userId)
            .Include(u => u.Borrows)
            .Select(u => u.Borrows.Select(br => br.BookId))
            .FirstOrDefaultAsync() ?? [];
    }
}
