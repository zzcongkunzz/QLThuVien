using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QLThuVien.Business.Models;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;
using static TorchSharp.torch.utils.data;

namespace QLThuVien.Business.ML;

public class BookRatingDataset : Dataset
{
    private IUnitOfWork _unitOfWork;
    public Dictionary<Guid, Tensor> UserTensors { get; private set; }
    public Dictionary<Guid, Tensor> BookTensors { get; private set; }
    public List<RatingVm> RatingVms { get; private set; }
    public FeatureExtractor Extractor { get; set; }
    public StandardScaler UserScaler { get; set; }
    public StandardScaler BookScaler { get; set; }

    public BookRatingDataset(IUnitOfWork unitOfWork, FeatureExtractor featureExtractor)
    {
        _unitOfWork = unitOfWork;
        Extractor = featureExtractor;
    }

    public override long Count => RatingVms.Count;

    public override Dictionary<string, Tensor> GetTensor(long index)
    {
        var dict = new Dictionary<string, Tensor>();

        var rating = RatingVms[(int)index];
        var userTensor = UserTensors[rating.UserId];
        var bookTensor = BookTensors[rating.BookId];

        dict.Add("rating", tensor(rating.Value, ScalarType.Float64));
        dict.Add("user", userTensor);
        dict.Add("book", bookTensor);

        return dict;
    }

    public async Task Initialize(int syntheticDataSize)
    {
        await Extractor.LoadCategoryIndices();

        UserTensors = [];
        BookTensors = [];
        RatingVms = [];

        // load users
        await foreach (var user in _unitOfWork.GetRepository<User>().GetQuery()
                .Include(u => u.Ratings)
                .Include(u => u.Borrows)
                .Include(u => u.FavoriteCategories)
                .AsAsyncEnumerable())
        {
            UserTensors.Add(user.Id, Extractor.GetTensor(user));
        }

        // load books
        await foreach (var book in _unitOfWork.GetRepository<Book>().GetQuery()
                .Include(b => b.Ratings)
                .Include(b => b.Borrows)
                .AsAsyncEnumerable())
        {
            BookTensors.Add(book.Id, Extractor.GetTensor(book));
        }

        // load ratings
        await foreach (var rating in _unitOfWork.GetRepository<Rating>()
            .GetQuery().AsAsyncEnumerable())
        {
            RatingVms.Add(new RatingVm()
            {
                UserId = rating.UserId,
                BookId = rating.BookId,
                Value = rating.Value
            });
        }

        // initialize scalers
        UserScaler = InitializeScaler(Extractor.UserTensorLength,
            FeatureExtractor.UserTensorScalableLength, UserTensors);
        BookScaler = InitializeScaler(Extractor.BookTensorLength,
            FeatureExtractor.BookTensorScalableLength, BookTensors);

        // scale real data
        foreach (var tensor in UserTensors.Values)
            UserScaler.Scale_(tensor);
        foreach (var tensor in BookTensors.Values)
            BookScaler.Scale_(tensor);

        // generate synthetic data
        if (syntheticDataSize > 0)
        {
            GenerateSyntheticData(syntheticDataSize, syntheticDataSize, 1);
        }
    }


    private void GenerateSyntheticData(int userMultiplier, int bookMultiplier, int ratingMultiplier)
    {
        var rand = new Random();
        int[] categoryIndices = Enumerable.Range(0, Extractor.CategoryIndices.Count).ToArray();

        // synthetic users count = usersMultiplier * categories * 5 * 2
        for (int i = 0; i < categoryIndices.Length; i++)
        {
            for (int j = 0; j < 5; ++j)
            {
                for (int k = 0; k < userMultiplier; ++k)
                {
                    var rating = normal(j + 0.5, 0.5).item<double>();
                    rating = Math.Max(rating, 0);
                    rating = Math.Min(rating, 5);
                    UserTensors.Add(Guid.NewGuid(),
                        Extractor.GetScaledUserTensor(rating, "male",
                            [categoryIndices[i]], UserScaler));
                    UserTensors.Add(Guid.NewGuid(),
                        Extractor.GetScaledUserTensor(rating, "female",
                        [categoryIndices[i]], BookScaler));
                }
            }
        }
        //for (int i = 0; i < userCount; ++i)
        //{
        //    var userAverageRating = rand.NextDouble() * 5;
        //    var userFavoriteCategoryIndices = rand.GetItems(categoryIndices, rand.Next(categoryIndices.Length));
        //    _userTensors.Add(Guid.NewGuid(), _featureExtractor.GetUserTensor(userAverageRating, "male", userFavoriteCategoryIndices));
        //    _userTensors.Add(Guid.NewGuid(), _featureExtractor.GetUserTensor(userAverageRating, "female", userFavoriteCategoryIndices));
        //}

        // synthetic books count = booksMultiplier * categories * 5
        for (int i = 0; i < categoryIndices.Length; i++)
        {
            int[] bookCategoryIndices = [categoryIndices[i]];
            for (int j = 0; j < 5; ++j)
            {
                for (int k = 0; k < bookMultiplier; ++k)
                {
                    var rating = normal(j + 0.5, 0.5).item<double>();
                    rating = Math.Max(rating, 0);
                    rating = Math.Min(rating, 5);
                    BookTensors.Add(
                        Guid.NewGuid(),
                        Extractor.GetScaledBookTensor(rating, bookCategoryIndices, BookScaler));
                }
            }
        }

        // synthetic ratings count = ratingMultiplier * synthUserCount * synthBookCount
        foreach (var userTensor in UserTensors)
        {
            foreach (var bookTensor in BookTensors)
            {
                for (int i = 0; i < ratingMultiplier; ++i)
                {
                    var ratingValue = (
                            (bookTensor.Value[1] * BookScaler.StandardDeviation[1]
                                + BookScaler.Mean[1]).item<double>() * .7
                            + (userTensor.Value[1] * UserScaler.StandardDeviation[1]
                                + UserScaler.Mean[1]).item<double>() * .3);
                    ratingValue *= (1 +
                            (bookTensor.Value
                                .slice(
                                    0,
                                    FeatureExtractor.BookTensorFixedLength,
                                    bookTensor.Value.shape[0] - 1,
                                    1)
                            .dot(userTensor.Value
                                .slice(
                                    0,
                                    FeatureExtractor.UserTensorFixedLength,
                                    userTensor.Value.shape[0] - 1,
                                    1)).item<double>() > 0 ? normal(0.8, 0.2).item<double>() 
                                        : normal(-0.2, 0.1).item<double>())
                            );
                    ratingValue = Math.Min(ratingValue, 5d);
                    ratingValue = Math.Max(ratingValue, 0d);
                    RatingVms.Add(new RatingVm()
                    {
                        UserId = userTensor.Key,
                        BookId = bookTensor.Key,
                        Value = ratingValue
                    });
                }
            }
        }
    }


    private StandardScaler InitializeScaler(int length, int scalableLength, Dictionary<Guid, Tensor> tensorDict)
    {
        var scaler = new StandardScaler()
        {
            Mean = zeros(length, ScalarType.Float64),
            StandardDeviation = zeros(length, ScalarType.Float64),
        };

        // calculate mean
        foreach (var userTensor in tensorDict.Values)
        {
            scaler.Mean.slice(0, 0, scalableLength, 1)
                .add_(userTensor.slice(0, 0, scalableLength, 1));
        }
        if (!tensorDict.IsNullOrEmpty())
            scaler.Mean.slice(0, 0,
                scalableLength, 1).div_(tensorDict.Count);

        // calculate standard deviation
        scaler.StandardDeviation.slice(0, 0, scalableLength, 1).zero_();
        foreach (var userTensor in tensorDict.Values)
        {
            scaler.StandardDeviation.slice(0, 0, scalableLength, 1)
                .add_(userTensor.slice(0, 0, scalableLength, 1)
                    .sub(scaler.Mean.slice(0, 0, scalableLength, 1))
                    .square());
        }
        if (!tensorDict.IsNullOrEmpty())
            scaler.StandardDeviation.slice(0, 0, scalableLength, 1)
                .div_(tensorDict.Count).sqrt_();
        // prevent division by zero
        for (int i = 0; i < scaler.StandardDeviation.shape[0]; i++)
            if (scaler.StandardDeviation[i].abs().item<double>() < 1e3)
                scaler.StandardDeviation[i] = 1;

        return scaler;
    }
}
