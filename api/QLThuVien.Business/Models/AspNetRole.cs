using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace QLThuVien.Data.Models;

public class AspNetRole: IdentityRole<Guid>
{
    [Required]
    public required string Description { get; set; }
};
