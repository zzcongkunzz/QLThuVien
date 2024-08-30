using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;

namespace QLThuVien.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController 
    (IUserService userService)
    : ControllerBase
{
    [HttpGet("get-all-users")]
    public async Task<ActionResult<IEnumerable<UserVm>>> GetAllUsers()
    {
        return Ok(await userService.GetAllAsyncVms());
    }

    [HttpGet("get-user-by-id/{id}")]
    public async Task<ActionResult<UserVm>> GetUserById(Guid id)
    {
        return Ok(await userService.GetByIdAsyncVm(id));
    }

    [HttpGet("query-users")]
    public async Task<ActionResult<IEnumerable<UserVm>>> QueryUser(
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

    [HttpPost("add-user")]
    public async Task<ActionResult> AddUser(UserCreateVm createUserVm)
    {
        await userService.CreateAsync(createUserVm);
        return Created();
    }

    [HttpPut("update-user/{id}")]
    public async Task<ActionResult> UpdateUser(Guid id, UserEditVm userVm)
    {
        await userService.UpdateAsync(id, userVm);
        return NoContent();
    }

    [HttpPut("change-password/{id}")]
    public async Task<ActionResult> ChangePassword(Guid id, ChangePasswordVm changePasswordVm)
    {
        await userService.ChangePasswordAsync(id, changePasswordVm.CurrentPassword, changePasswordVm.NewPassword);
        return NoContent();
    }

    [HttpDelete("delete-user/{id}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        await userService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("add-favorite-category")]
    public async Task<ActionResult> AddFavoriteCategory(
        [FromQuery] Guid userId,
        [FromQuery] Guid categoryId
        )
    {
        await userService.AddFavoriteCategoryAsync(userId, categoryId);
        return NoContent();
    }

    [HttpDelete("delete-favorite-category")]
    public async Task<ActionResult> DeleteFavoriteCategory(
        [FromQuery] Guid userId,
        [FromQuery] Guid categoryId
        )
    {
        await userService.DeleteFavoriteCategoryAsync(userId, categoryId);
        return NoContent();
    }

    [HttpGet("get-favorite-categories/{userId}")]
    public async Task<ActionResult<Category>> GetFavoriteCategories(Guid userId)
    {
        return Ok(await userService.GetFavoriteCategoriesAsync(userId));
    }
}


