using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using QLThuVien.Data.Models;

namespace QLThuVien.Business.Models;

public class Borrow
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public User User { get; set; }
    public Book Book { get; set; }
    [Required]
    public required DateTime StartTime { get; set; }
    [Required]
    public required DateTime ExpectedReturnTime { get; set; }
    public DateTime? ActualReturnTime { get; set; }
    [Required]
    public required int Count { get; set; }
    [Required, DefaultValue(0f)]
    public required float IssuedPenalties { get; set; }
    [Required, DefaultValue(0f)]
    public required float PaidPenalties { get; set; }
}