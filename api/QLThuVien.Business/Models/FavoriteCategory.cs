using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using QLThuVien.Data.Models;

namespace QLThuVien.Business.Models;

[PrimaryKey(nameof(UserId), nameof(CategoryId))]
public class FavoriteCategory
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    [Required]
    public User User { get; set; }
    [Required]
    public Category Category { get; set; }
}