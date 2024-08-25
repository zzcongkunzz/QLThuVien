using System.ComponentModel.DataAnnotations;
using QLThuVien.Data.Models;

namespace QLThuVien.Business.Models;

public class FavoriteCategory
{
    [Required]
    public User User { get; set; }
    [Required]
    public Category Category { get; set; }
}