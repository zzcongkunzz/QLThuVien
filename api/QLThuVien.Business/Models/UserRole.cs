using Microsoft.AspNetCore.Identity;
using QLThuVien.Data.Models;

namespace QLThuVien.Business.Models;

public class UserRole : IdentityUserRole<Guid>
{
    public User User { get; set; }
    public Role Role { get; set; }
}
