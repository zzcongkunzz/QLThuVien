using Microsoft.EntityFrameworkCore;

namespace QLThuVien.Data.Models;

[Index(nameof(Name), nameof(LastUpdated), IsUnique = true)]
public class RawMLModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime LastUpdated { get; set; }
    public int Version { get; set; }
    public bool IsValid { get; set; }
    public byte[] RawModule { get; set; }
    public RawFeatureExtractor RawFeatureExtractor { get; set; }
}
