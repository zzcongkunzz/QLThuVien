using System.ComponentModel.DataAnnotations;
using QLThuVien.Data.Models;

namespace QLThuVien.Business.Models;

public class FavoriteBook
{
    [Required]
    public User User { get; set; }
    [Required]
    public Book Book { get; set; }
}