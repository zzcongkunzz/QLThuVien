namespace QLThuVien.Business.ViewModels;

public class UserVm
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Gender { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required string FullName { get; set; }
    public string Role { get; set; }
}
