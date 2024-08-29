using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;

namespace QLThuVien.Business.Services.Implementations;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthenticationService(UserManager<User> userManager, RoleManager<Role> roleManager, IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<AuthResultVM> Login(LoginVM payload)
    {
        var user = await _userManager.FindByEmailAsync(payload.Email);
        
        if (user != null && await _userManager.CheckPasswordAsync(user, payload.Password))
        {
            var tokenValue = await GenerateJwtToken(user);
            return tokenValue;
        }
        return new AuthResultVM 
        {
            Token = null,
            RefreshToken = null
        };
    }
    
    private async Task<AuthResultVM> GenerateJwtToken(User user)
    {
        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            expires: DateTime.UtcNow.AddMinutes(10), // 5 - 10mins
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        
        var refreshToken = new RefreshToken()
        {
            JwtId = token.Id,
            IsRevoked = false,
            UserId = user.Id,
            DateAdded = DateTime.UtcNow,
            DateExpire = DateTime.UtcNow.AddMonths(6),
            Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
        };
        _unitOfWork.GetRepository<RefreshToken>().Add(refreshToken);
        await _unitOfWork.SaveChangesAsync();
        var response = new AuthResultVM()
        {
            Token = jwtToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = token.ValidTo,
            UserInformation = new UserVm()
            {
                Id = user.Id,
                Email = user.Email,
                Gender = user.Gender,
                // Role = user?.Roles.ToString(),
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
            }
        };
        return response;
    }
}