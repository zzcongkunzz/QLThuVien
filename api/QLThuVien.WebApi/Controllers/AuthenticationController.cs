using Microsoft.AspNetCore.Mvc;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;

namespace QLThuVien.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginVm payload)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Please, provide all required fields");
        }
        AuthResultVm result = await _authenticationService.Login(payload);
        if (result.Token == null)
        {
            return Unauthorized();
        }
        return Ok(result);
    }
}