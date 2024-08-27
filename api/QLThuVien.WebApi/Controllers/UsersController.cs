using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLThuVien.Business.Services.Implementations;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Models;

namespace QLThuVien.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController 
    (IUserService userService)
    : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateUser(CreateUserVm createUserVm)
    {
        await userService.CreateAsync(createUserVm);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(string id, UserVm userVm)
    {
        await userService.UpdateAsync(new Guid(id), userVm);
        return Ok();
    }

    [HttpPut("{id}/change-password")]
    public async Task<ActionResult> ChangePassword(string id, ChangePasswordVm changePasswordVm)
    {
        await userService.ChangePasswordAsync(new Guid(id), changePasswordVm.CurrentPassword, changePasswordVm.NewPassword);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        await userService.DeleteAsync(new Guid(id));
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(string id)
    {
        await userService.DeleteAsync(new Guid(id));
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserVm>>> Get(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string name = "",
        [FromQuery] string order = "FullName"
        )
    {
        return Ok(await userService.GetAsyncVm
            (
                pageIndex,
                pageSize,
                "Roles",
                u => u.FullName.Contains(name),
                q => order switch
                {
                    _ => q.OrderBy(u => u.FullName)
                }
            ));
    }
}
