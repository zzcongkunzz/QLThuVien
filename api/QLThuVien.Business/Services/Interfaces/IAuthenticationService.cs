using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Models;

namespace QLThuVien.Business.Services.Interfaces;

public interface IAuthenticationService
{
    Task<AuthResultVM> Login(LoginVM payload);
}