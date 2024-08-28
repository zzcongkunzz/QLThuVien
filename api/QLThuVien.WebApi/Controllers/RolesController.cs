using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;

namespace QLThuVien.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DefaultsController 
    (IRoleService roleService, IHostEnvironment hostEnvironment)
    : ControllerBase
{
    [HttpGet("get-admin-account")]
    public IActionResult GetAdminAccount()
    {
        // For development only
        if (hostEnvironment.IsDevelopment())
            return Ok(new { Email = "admin@gmail.com", Password = "Admin_123" });
        return Unauthorized();
    }

    [HttpGet("get-all-roles")]
    public async Task<ActionResult<IEnumerable<RoleVm>>> GetRoles()
    {
        return Ok(await roleService.GetAllAsyncVm());
    }
}
