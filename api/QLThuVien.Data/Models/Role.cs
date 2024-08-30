using Microsoft.AspNetCore.Identity;
namespace QLThuVien.Data.Models;

public class Role: IdentityRole<Guid>
{
    public string? Description { get; set; }
    public IEnumerable<User> Users { get; set; }
};
