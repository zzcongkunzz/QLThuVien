using static TorchSharp.torch;

namespace QLThuVien.Business.ML;

public class StandardScaler
{
    public Tensor Mean { get; set; }
    public Tensor StandardDeviation { get; set; }

    public Tensor Scale_(Tensor input)
    {
        return input.sub_(Mean).div_(StandardDeviation);
    }
}
