using QLThuVien.Business.ML;
using System.Linq.Expressions;
using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;
using static TorchSharp.torch.utils.data;

namespace QLThuVien.Business.MLModules;

public class TwoTowerModule : Module<Tensor, Tensor, Tensor>
{
    public static readonly string Name = "TwoTowerModule";
    private readonly Module<Tensor, Tensor> _userNetwork;
    private readonly Module<Tensor, Tensor> _itemNetwork;
    private readonly Tensor _weights;
    private static readonly int[] _layerSizes = [128, 64, 32];
    private const double _l2RegFactor = 0.001;

    public bool IsValid { get; set; } = false;

    public TwoTowerModule(int userTensorLength, int itemTensorLength) : base(Name)
    {
        Linear[] userLins = [
            Linear(userTensorLength, _layerSizes[0], dtype: ScalarType.Float64),
            .. Enumerable.Range(0, _layerSizes.Length - 1)
                .Select(i => Linear(_layerSizes[i], _layerSizes[i+1], dtype: ScalarType.Float64))];
        Linear[] itemLins = [
            Linear(itemTensorLength, _layerSizes[0], dtype: ScalarType.Float64),
            .. Enumerable.Range(0, _layerSizes.Length - 1)
                .Select(i => Linear(_layerSizes[i], _layerSizes[i+1], dtype: ScalarType.Float64))];

        _userNetwork = Sequential(
            Enumerable.Range(0, _layerSizes.Length).SelectMany(i => new Module<Tensor, Tensor>[]
                { userLins[i], ReLU(), Dropout(.1) }));

        _itemNetwork = Sequential(
            Enumerable.Range(0, _layerSizes.Length).SelectMany(i => new Module<Tensor, Tensor>[]
                { itemLins[i], ReLU(), Dropout(.1) }));

        RegisterComponents();

        _weights = concat(
            userLins.Select(lin => lin.weight!.flatten())
            .Concat(itemLins.Select(lin => lin.weight!.flatten()))
            .ToList());
    }

    public override Tensor forward(Tensor userTensor, Tensor itemTensor)
    {
        return bmm(
            _userNetwork.forward(userTensor).reshape([-1, 1, _layerSizes.Last()]), 
            _itemNetwork.forward(itemTensor).reshape([-1, _layerSizes.Last(), 1]));
    }

    public double predict(Tensor userTensor, Tensor itemTensor)
    {
        eval();
        using (no_grad())
        {
            return forward(userTensor, itemTensor).item<double>();
        }
    }

    public List<double> fit(
        BookRatingDataset dataset,
        int epochs = 20,
        int batchSize = 128)
    {
        train();
        using var outerScope = NewDisposeScope();

        List<double> costs = [];
        var optimizer = optim.Adam(parameters(true));
        var dataLoader = DataLoader(dataset, batchSize, shuffle: true);

        for (int i = 0; i < epochs; ++i)
        {
            double epochCost = 0;

            foreach (var batch in dataLoader)
            {
                using var batchScope = NewDisposeScope();

                var J = functional.mse_loss(
                        forward(batch["user"], batch["book"]).flatten(),
                        batch["rating"].flatten()) 
                    + _weights.square().sum().mul(_l2RegFactor);

                zero_grad();
                J.backward();
                optimizer.step();

                epochCost += J.item<double>();
            }

            costs.Add(epochCost / dataLoader.Count);
        }

        using (no_grad())
        {
            double finalCost = 0;
            foreach (var batch in dataLoader)
            {
                using var batchScope = NewDisposeScope();

                var J = functional.mse_loss(
                        forward(batch["user"], batch["book"]).flatten(),
                        batch["rating"].flatten())
                    + _weights.square().sum().mul(_l2RegFactor);

                finalCost += J.item<double>();
            }
            costs.Add(finalCost / dataLoader.Count);
        }

        return costs;
    }


    public Tensor GetItemEmbedding(Tensor itemTensor)
    {
        eval();
        using (no_grad())
        {
            return _itemNetwork.forward(itemTensor);
        }
    }


    public Tensor GetUserEmbedding(Tensor userTensor)
    {
        eval();
        using (no_grad())
        {
            return _itemNetwork.forward(userTensor);
        }
    }


    public double GetR2Score(BookRatingDataset dataset)
    {
        double resSS = 0, totalSS = 0, mean = 0;
        eval();
        using(no_grad())
        {
            for (int i = 0; i < dataset.Count; ++i)
            {
                var tensor = dataset.GetTensor(i);
                var rating = tensor["rating"].item<double>();
                var res = rating - predict(tensor["user"], tensor["book"]);
                resSS += res * res;
                mean += rating;
            }
            if (dataset.Count > 0)
                mean /= dataset.Count;
            for (int i = 0; i < dataset.Count; ++i)
            {
                var tensor = dataset.GetTensor(i);
                var rating = tensor["rating"].item<double>();
                var total = rating - mean;
                totalSS += total * total;
            }
        }
        return 1 - resSS / (totalSS > 0 ? totalSS : 1);
    }
}
