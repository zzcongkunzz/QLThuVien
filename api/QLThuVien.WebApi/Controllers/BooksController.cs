using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLThuVien.Business.Services.Implementations;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;

namespace QLThuVien.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController 
    (IBookService bookService)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserVm>>> Get(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string title = "",
        [FromQuery] string order = "Title"
        )
    {
        return Ok(await bookService.GetAsyncVm
            (
                pageIndex,
                pageSize,
                "Category",
                u => u.Title.Contains(title),
                q => order switch
                {
                    _ => q.OrderBy(b => b.Title)
                }
            ));
    }

    [HttpPost]
    public async Task<ActionResult> CreateBook(BookVm bookVm)
    {
        await bookService.AddAsync(bookVm);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBook(string id)
    {
        await bookService.DeleteAsync(new Guid(id));
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBook(string id, BookVm bookVm)
    {
        await bookService.UpdateAsync(new Guid(id), bookVm);
        return Ok();
    }
}
