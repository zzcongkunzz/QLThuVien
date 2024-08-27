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
    public async Task<ActionResult> CreateUser(UserCreateVm createUserVm)
    {
        await userService.CreateAsync(createUserVm);
        return Created();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(Guid id, UserEditVm userVm)
    {
        await userService.UpdateAsync(id, userVm);
        return Ok();
    }

    [HttpPut("{id}/change-password")]
    public async Task<ActionResult> ChangePassword(Guid id, ChangePasswordVm changePasswordVm)
    {
        await userService.ChangePasswordAsync(id, changePasswordVm.CurrentPassword, changePasswordVm.NewPassword);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        await userService.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserVm>> GetById(Guid id)
    {
        return Ok(await userService.GetByIdAsyncVm(id));
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
