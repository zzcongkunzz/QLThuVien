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
    public async Task<ActionResult> Add(Category category)
    {
        await categoryService.AddAsync(category);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        await categoryService.DeleteAsync(new Guid(id));
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, Category category)
    {
        category.Id = new Guid(id);
        await categoryService.UpdateAsync(category);
        return Ok();
    }
}
