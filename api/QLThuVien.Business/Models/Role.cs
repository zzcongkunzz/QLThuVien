using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
namespace QLThuVien.Data.Models;

public class Role: IdentityRole<Guid>
{
    public string? Description { get; set; }
    public List<User> Users { get; set; }
};
