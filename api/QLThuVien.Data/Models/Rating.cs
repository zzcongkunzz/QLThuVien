using System.ComponentModel.DataAnnotations;
using QLThuVien.Data.Models;

namespace QLThuVien.Business.Models;

public class Rating
{
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public User User { get; set; }
    
    public Book Book { get; set; }
    
    [Required, Range(0d, 5d)]
    public float Value { get; set; }
}