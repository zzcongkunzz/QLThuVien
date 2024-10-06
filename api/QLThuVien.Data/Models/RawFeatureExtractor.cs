namespace QLThuVien.Data.Models;

public class RawFeatureExtractor
{
    public Guid Id { get; set; }
    public Guid RawMLModelId { get; set; }
    public byte[] UserMean { get; set; }
    public byte[] UserStandardDeviation { get; set; }
    public byte[] BookMean { get; set; }
    public byte[] BookStandardDeviation { get; set; }
    public RawMLModel RawMLModel { get; set; }
    public IList<CategoryIndex> CategoryIndices { get; set; }
}
