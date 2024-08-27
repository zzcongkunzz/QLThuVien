namespace QLThuVien.Business.ViewModels;

public class UserEditVm
{
    public required string Email { get; set; }
    public required string Gender { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required string FullName { get; set; }
    public required string Role { get; set; }
}
