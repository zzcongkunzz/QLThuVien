using Microsoft.AspNetCore.Mvc;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;

namespace QLThuVien.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class CategoriesController
    (ICategoryService categoryService)
    : ControllerBase
{
    [HttpGet("get-all-categories")]
    public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
    {
        return Ok(await categoryService.GetAllAsync());
    }

    [HttpPost("add-category")]
    public async Task<ActionResult> AddCategory(CategoryEditVm categoryEditVm)
    {
        await categoryService.AddAsync(categoryEditVm);
        return Created();
    }

    [HttpDelete("get-category-by-id/{id}")]
    public async Task<ActionResult<Category>> GetCategoryById(Guid id)
    {
        return Ok(await categoryService.GetByIdAsync(id));
    }

    [HttpDelete("delete-category/{id}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        await categoryService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("update-category/{id}")]
    public async Task<ActionResult> UpdateCategory(Guid id, CategoryEditVm category)
    {
        await categoryService.UpdateAsync(id, category);
        return NoContent();
    }
}
