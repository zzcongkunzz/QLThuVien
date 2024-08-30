using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using QLThuVien.Data.Models;

namespace QLThuVien.Business.Models;

[Index(nameof(UserId), nameof(BookId))]
public class FavoriteBook
{
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    [Required]
    public User User { get; set; }
    [Required]
    public Book Book { get; set; }
}