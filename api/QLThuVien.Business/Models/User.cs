using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace QLThuVien.Data.Models;

public class User : IdentityUser<Guid>
{
    [Required]
    public required string Gender { get; set; }
    [Required]
    public required DateTime DateOfBirth { get; set; }
    [Required]
    public required string FullName { get; set; }


    public Guid RoleId { get; set; }
    public Role Role { get; set; }
}
