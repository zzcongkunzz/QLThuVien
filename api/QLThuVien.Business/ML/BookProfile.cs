namespace QLThuVien.Business.ML;

public class BookProfile
{
    public DateOnly PublishDate { get; set; }
    public int Count { get; set; }
    public int TotalBorrowCount { get; set; }
    public int RemainingCount { get; set; }
    public double AverageRating { get; set; }
    public IList<Guid> CategoryIds { get; set; }
}
