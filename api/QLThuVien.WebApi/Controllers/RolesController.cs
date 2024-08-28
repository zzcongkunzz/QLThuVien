using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;

namespace QLThuVien.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController
    (IRoleService roleService)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleVm>>> GetRoles()
    {
        return Ok(await roleService.GetAllAsyncVm());
    }
}
