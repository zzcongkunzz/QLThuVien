using Microsoft.AspNetCore.Identity;
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
}
