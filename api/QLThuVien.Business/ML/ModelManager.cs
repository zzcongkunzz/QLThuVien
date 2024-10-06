using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QLThuVien.Business.MLModules;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;
using System.Text.Json;
using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;

namespace QLThuVien.Business.ML;

public class ModelManager
{
    private readonly IServiceScopeFactory _scopeFactory;
    public StandardScaler? UserScaler { get; private set; } = null;
    public StandardScaler? BookScaler { get; private set; } = null;
    public FeatureExtractor? FeatureExtractor { get; private set; } = null;
    public TwoTowerModule? Module { get; private set; } = null;
    public bool IsInitialized => 
        Module != null && FeatureExtractor != null && UserScaler != null && BookScaler != null;

    public ModelManager(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }


    public async Task<TrainResult> Train(int syntheticDataSize, IServiceProvider services)
    {
        var dataset = services.GetService<BookRatingDataset>()
            ?? throw new Exception("Unregistered service");

        await dataset.Initialize(syntheticDataSize);

        Module = new TwoTowerModule(
            dataset.Extractor.UserTensorLength,
            dataset.Extractor.BookTensorLength
            );
        FeatureExtractor = dataset.Extractor;
        UserScaler = dataset.UserScaler;
        BookScaler = dataset.BookScaler;

        var startTime = DateTime.Now;
        var costs = Module.fit(dataset);
        var elapsedSeconds = DateTime.Now.Subtract(startTime).TotalSeconds;

        SaveModelToDisk();

        byte[] buffer;
        using (var writer = new MemoryStream())
        {
            Module.save(writer);
            buffer = writer.GetBuffer();
        }
        return new TrainResult()
        {
            ElapsedSeconds = elapsedSeconds,
            ParametersCount = Module.parameters().Sum(p => p.numel()),
            SizeInBytes = buffer.LongLength,
            UsersCount = dataset.UserTensors.Count,
            BooksCount = dataset.BookTensors.Count,
            RatingsCount = dataset.RatingVms.Count,
            Costs = costs,
        };
    }

    public void SaveModelToDisk()
    {
        if (Module == null || UserScaler == null || BookScaler == null
            || FeatureExtractor == null)
            throw new ArgumentNullException("Model must be loaded");

        Directory.CreateDirectory("ml_data");

        Module.save("ml_data/two_tower_module.dat");
        UserScaler.Mean.save("ml_data/user_mean.dat");
        UserScaler.StandardDeviation.save("ml_data/user_standard_deviation.dat");
        BookScaler.Mean.save("ml_data/book_mean.dat");
        BookScaler.StandardDeviation.save("ml_data/book_standard_deviation.dat");

        using (var stream = new FileStream("ml_data/category_indices.dat", FileMode.Create, FileAccess.Write))
            JsonSerializer.Serialize(stream, FeatureExtractor.CategoryIndices);
    }

    public long LoadModelFromDisk()
    {

        using (var stream = new FileStream("ml_data/category_indices.dat", FileMode.Open, FileAccess.Read))
            FeatureExtractor = new FeatureExtractor(
                JsonSerializer.Deserialize<Dictionary<Guid, int>>(stream)
                ?? throw new OperationCanceledException("Can't deserialize category_indices.dat"));

        UserScaler ??= new StandardScaler();
        UserScaler.Mean = load("ml_data/user_mean.dat");
        UserScaler.StandardDeviation = load("ml_data/user_standard_deviation.dat");
        BookScaler ??= new StandardScaler();
        BookScaler.Mean = load("ml_data/book_mean.dat");
        BookScaler.StandardDeviation = load("ml_data/book_standard_deviation.dat");

        Module = new TwoTowerModule(FeatureExtractor.UserTensorLength,
            FeatureExtractor.BookTensorLength);
        Module.load("ml_data/two_tower_module.dat");

        byte[] buffer;
        using (var writer = new MemoryStream())
        {
            Module.save(writer);
            buffer = writer.GetBuffer();
        }
        return buffer.LongLength;
    }

    private async Task<RawMLModel?> GetCurrentModel(IUnitOfWork unitOfWork)
    {
        return await unitOfWork.GetRepository<RawMLModel>().GetQuery()
                .Where(m => m.Name == TwoTowerModule.Name && m.IsValid)
                .OrderByDescending(m => m.Version)
                .Include(m => m.RawFeatureExtractor)
                .ThenInclude(e => e.CategoryIndices)
                .FirstOrDefaultAsync();
    }


    //public async Task<bool> LoadModelAsync()
    //{
    //RawMLModel? rawModel = null;
    //using (var scope = _scopeFactory.CreateScope())
    //{
    //    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    //    rawModel = await GetCurrentModel(unitOfWork);
    //    if (rawModel == null)
    //        return false;

    //    int categoryCnt = await unitOfWork.GetRepository<Category>().GetQuery().CountAsync();
    //    _module = new TwoTowerModule(categoryCnt, categoryCnt);
    //}

    //using (var reader = new MemoryStream(rawModel.RawModule))
    //{
    //    _module!.load(reader);
    //    _module.IsValid = true;
    //}

    //    return true;
    //}

    //public async Task SaveModelAsync(
    //    TwoTowerModule module,
    //    FeatureExtractor featureExtractor,
    //    StandardScaler userScaler,
    //    StandardScaler bookScaler)
    //{
    //byte[] buffer;

    //using (var writer = new MemoryStream())
    //{
    //    module.save(writer);
    //    buffer = writer.GetBuffer();
    //}

    //using (var scope = _scopeFactory.CreateScope())
    //{
    //var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    //var _rawModel = await GetCurrentModel(unitOfWork);
    //if (_rawModel == null)
    //{
    //_rawModel = new RawMLModel()
    //{
    //    Name = TwoTowerModule.Name,
    //    RawModule = buffer,
    //    LastUpdated = DateTime.Now,
    //    RawFeatureExtractor = new RawFeatureExtractor()
    //    {
    //        BookMean = featureExtractor.
    //    },
    //    IsValid = true
    //};
    //unitOfWork.GetRepository<RawMLModel>().Add(_rawModel);
    //}
    //else
    //{
    //    _rawModel.RawModule = buffer;
    //    _rawModel.LastUpdated = DateTime.Now;
    //    unitOfWork.GetRepository<RawMLModel>().Update(_rawModel);
    //}
    //await unitOfWork.SaveChangesAsync();
    //}
    //}
}
