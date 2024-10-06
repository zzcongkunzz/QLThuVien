using Microsoft.EntityFrameworkCore;
using QLThuVien.Business.Models;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;
using System.Reflection;
using static TorchSharp.torch;

namespace QLThuVien.Business.ML;

public class FeatureExtractor
{
    private IUnitOfWork _unitOfWork;
    private DateTime _now = DateTime.Now;
    public static int UserTensorFixedLength = 6;
    public static int BookTensorFixedLength = 5;
    public Dictionary<Guid, int> CategoryIndices { get; set; }
    public static int UserTensorScalableLength => UserTensorFixedLength - 2;
    public static int BookTensorScalableLength => BookTensorFixedLength;
    public int UserTensorLength => UserTensorFixedLength + CategoryIndices.Count;
    public int BookTensorLength => BookTensorFixedLength + CategoryIndices.Count;
    public static string UserIncludeProperties = "Borrows,Ratings,FavoriteCategories";
    public static string BookIncludeProperties = "Borrows,Ratings,Category";

    public FeatureExtractor(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public FeatureExtractor(Dictionary<Guid, int> categoryIndices)
    {
        CategoryIndices = categoryIndices;
    }

    public async Task LoadCategoryIndices()
    {
        CategoryIndices = [];

        // load all categories and map to the correct index in feature vector
        var categoryIds = await _unitOfWork.GetRepository<Category>().GetQuery()
            .Select(c => c.Id).ToArrayAsync();
        foreach (var id in categoryIds)
            CategoryIndices.Add(id, CategoryIndices.Count);
    }


    public Tensor GetTensor(User user)
    {
        return GetTensor(GetProfile(user));
    }

    public Tensor GetTensor(Book book)
    {
        return GetTensor(GetProfile(book));
    }


    public Tensor GetScaledUserTensor(double averageRating, string gender, IList<int> favoriteCategoryIndices, StandardScaler scaler)
    {
        var ouput = zeros(UserTensorFixedLength + CategoryIndices.Count, ScalarType.Float64);

        ouput[1] = averageRating / scaler.Mean[1];

        foreach (var catIdx in favoriteCategoryIndices)
            ouput[catIdx + UserTensorFixedLength] = 1;

        ouput[4] = (gender == "male" ? 1d : 0d);
        ouput[5] = (gender == "female" ? 1d : 0d);

        return ouput;
    }

    public Tensor GetScaledBookTensor(double averageRating, IList<int> categoryIndices, StandardScaler scaler)
    {
        var ouput = zeros(BookTensorFixedLength + CategoryIndices.Count, ScalarType.Float64);

        ouput[1] = (averageRating - scaler.Mean[1]) / scaler.StandardDeviation[1];

        foreach (var catIdx in categoryIndices)
            ouput[catIdx + BookTensorFixedLength] = 1;

        return ouput;
    }


    private Tensor GetTensor(UserProfile userProfile)
    {
        IList<double> features = [
                _now.Subtract(userProfile.DateOfBirth.ToDateTime(TimeOnly.MinValue)).TotalDays,
                userProfile.AverageRating,
                userProfile.CurrentBorrowCount,
                userProfile.TotalBorrowCount,
                userProfile.Gender == "male" ? 1d : 0d,
                userProfile.Gender == "female" ? 1d : 0d,
                .. Enumerable.Repeat(0d, CategoryIndices.Count),
            ];
        foreach (var catId in userProfile.FavoriteCategoryIds)
        {
            if (CategoryIndices.TryGetValue(catId, out var idx))
                features[idx + UserTensorFixedLength] = 1;
        }
        return tensor(features, ScalarType.Float64);
    }

    private Tensor GetTensor(BookProfile bookProfile)
    {
        IList<double> features = [
                _now.Subtract(bookProfile.PublishDate.ToDateTime(TimeOnly.MinValue)).TotalDays,
                bookProfile.AverageRating,
                bookProfile.Count,
                bookProfile.TotalBorrowCount,
                bookProfile.RemainingCount,
                .. Enumerable.Repeat(0d, CategoryIndices.Count),
            ];
        foreach (var catId in bookProfile.CategoryIds)
        {
            if (CategoryIndices.TryGetValue(catId, out var idx))
                features[idx + BookTensorFixedLength] = 1;
        }
        return tensor(features, ScalarType.Float64);
    }


    private UserProfile GetProfile(User user)
    {
        return new UserProfile
        {
            Gender = user.Gender,
            DateOfBirth = user.DateOfBirth,
            CurrentBorrowCount = user.Borrows
                    .Where(b => b.ActualReturnTime != null && b.ActualReturnTime < DateTime.Now)
                    .Sum(b => b.Count),
            TotalBorrowCount = user.Borrows.Sum(b => b.Count),
            AverageRating = user.Ratings != null && user.Ratings.Any()
                    ? user.Ratings.Average(rating => rating.Value)
                    : 2.5,
            FavoriteCategoryIds = user.FavoriteCategories
                    .Select(c => c.Id).ToList()
        };
    }


    private BookProfile GetProfile(Book book)
    {
        return new BookProfile()
        {
            PublishDate = book.PublishDate,
            Count = book.Count,
            TotalBorrowCount = book.Borrows.Sum(b => b.Count),
            AverageRating = book.Ratings != null && book.Ratings.Any()
                        ? book.Ratings.Average(rating => rating.Value)
                        : 2.5,
            RemainingCount = book.Count - book.Borrows
                        .Where(borrow =>
                            borrow.ActualReturnTime == null || borrow.ActualReturnTime > DateTime.Now)
                        .Sum(borrow => borrow.Count),
            CategoryIds = [book.CategoryId]
        };
    }
}
