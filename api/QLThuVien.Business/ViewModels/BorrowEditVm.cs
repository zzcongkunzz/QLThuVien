using QLThuVien.Business.Models;
using QLThuVien.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace QLThuVien.Business.ViewModels;

public class BorrowEditVm
{
    public required Guid UserId { get; set; }
    public required Guid BookId { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime ExpectedReturnTime { get; set; }
    public required int Count { get; set; }
    public required float IssuedPenalties { get; set; }
    public required float PaidPenalties { get; set; }
}
