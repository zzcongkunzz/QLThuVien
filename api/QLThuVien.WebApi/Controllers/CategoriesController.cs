using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Implementations;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;

namespace QLThuVien.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController 
    (ICategoryService categoryService)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> Get()
    {
        return Ok(await categoryService.GetAllAsync());
    }

    [HttpPost]
    public async Task<ActionResult> Add(CategoryEditVm categoryEditVm)
    {
        await categoryService.AddAsync(categoryEditVm);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await categoryService.DeleteAsync(id);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, CategoryEditVm category)
    {
        await categoryService.UpdateAsync(id, category);
        return Ok();
    }
}
