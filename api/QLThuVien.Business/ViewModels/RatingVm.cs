using System.ComponentModel.DataAnnotations;

namespace QLThuVien.Business.ViewModels;

public class RatingVm
{
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    [Required, Range(0d, 5d)]
    public double Value { get; set; }
}
