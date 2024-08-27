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
    public async Task<ActionResult<IEnumerable<BookVm>>> Get(
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

    [HttpGet("{id}")]
    public async Task<ActionResult<BookVm>> GetById(Guid id)
    {

        return Ok(await bookService.GetByIdAsyncVm(id));
    }

    [HttpPost]
    public async Task<ActionResult> CreateBook(BookEditVm bookEditVm)
    {
        await bookService.AddAsync(bookEditVm);
        return Created();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBook(Guid id)
    {
        await bookService.DeleteAsync(id);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBook(Guid id, BookEditVm bookEditVm)
    {
        await bookService.UpdateAsync(id, bookEditVm);
        return Ok();
    }
}
