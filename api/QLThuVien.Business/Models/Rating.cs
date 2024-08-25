using System.ComponentModel.DataAnnotations;
using QLThuVien.Data.Models;

namespace QLThuVien.Business.Models;

public class Rating
{
    public User User { get; set; }
    
    public Book Book { get; set; }
    
    [Required]
    public float Value { get; set; }
}