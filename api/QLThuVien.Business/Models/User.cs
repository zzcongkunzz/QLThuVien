using Microsoft.AspNetCore.Identity;
using QLThuVien.Business.Models;
using System.ComponentModel.DataAnnotations;
namespace QLThuVien.Data.Models;

public class User : IdentityUser<Guid>
{
    [Required]
    public required string Gender { get; set; }
    [Required]
    public required DateOnly DateOfBirth { get; set; }
    [Required]
    public required string FullName { get; set; }
    public IEnumerable<Role> Roles { get; set; }
    public IEnumerable<Book> BorrowingBooks { get; set; }
    public IEnumerable<Category> FavoriteCategories { get; set; }
}
