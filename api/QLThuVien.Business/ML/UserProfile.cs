using System.Collections.Immutable;

namespace QLThuVien.Business.ML;

public class UserProfile
{
    public DateOnly DateOfBirth { get; set; }
    public string Gender{ get; set; }
    public double AverageRating { get; set; }
    public int CurrentBorrowCount { get; set; }
    public int TotalBorrowCount { get; set; }
    public IList<Guid> FavoriteCategoryIds { get; set; }
}
