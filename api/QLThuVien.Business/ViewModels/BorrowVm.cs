namespace QLThuVien.Business.ViewModels;

public class BorrowVm
{
    public Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required Guid BookId { get; set; }
    public required string UserFullName { get; set; }
    public required string BookTitle { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime ExpectedReturnTime { get; set; }
    public DateTime? ActualReturnTime { get; set; }
    public required int Count { get; set; }
    public required float IssuedPenalties { get; set; }
    public required float PaidPenalties { get; set; }
}
