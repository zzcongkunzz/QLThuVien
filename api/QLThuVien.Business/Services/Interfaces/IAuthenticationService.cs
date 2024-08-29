using QLThuVien.Business.ViewModels;

namespace QLThuVien.Business.Services.Interfaces;

public interface IAuthenticationService
{
    Task<AuthResultVm> Login(LoginVm payload);
}