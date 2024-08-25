using System.ComponentModel.DataAnnotations;
using QLThuVien.Data.Models;

namespace QLThuVien.Business.Models;

public class Borrow
{
    public Guid Id { get; set; }
    [Required]
    public User User { get; set; }
    [Required]
    public Book Book { get; set; }
    [Required]
    public string Status { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime ExpectedReturnDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }
}