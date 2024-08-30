using Microsoft.AspNetCore.Mvc;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;

namespace QLThuVien.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserService _userService;

    public AuthenticationController(IAuthenticationService authenticationService, IUserService userService)
    {
        _authenticationService = authenticationService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginVm loginVm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid login");
        }
        AuthResultVm result = await _authenticationService.Login(loginVm);
        if (result.Token == null)
        {
            return Unauthorized();
        }
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateVm userCreateVm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid register");
        }

        userCreateVm.Role = "member";
        await _userService.CreateAsync(userCreateVm);

        AuthResultVm result = await _authenticationService.Login(new LoginVm()
        {
            Email = userCreateVm.Email,
            Password = userCreateVm.Password
        });
        if (result.Token == null)
        {
            return Unauthorized();
        }
        return Ok(result);
    }
}