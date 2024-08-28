using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLThuVien.Business.Services.Implementations;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;

namespace QLThuVien.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BooksController 
    (IBookService bookService)
    : ControllerBase
{
    [HttpGet("get-all-books")]
    public async Task<ActionResult<IEnumerable<BookVm>>> GetAllBooks()
    {
        return Ok(await bookService.GetAll());
    }

    [HttpGet("query-books")]
    public async Task<ActionResult<PaginatedResult<BookVm>>> QueryBooks(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string title = "",
        [FromQuery] string order = "Title"
        )
    {
        return Ok(await bookService.QueryAsyncVm
            (
                pageIndex,
                pageSize,
                u => u.Title.Contains(title),
                q => order switch
                {
                    _ => q.OrderBy(b => b.Title)
                }
            ));
    }

    [HttpGet("get-book-by-id/{id}")]
    public async Task<ActionResult<BookVm>> GetBookById(Guid id)
    {

        return Ok(await bookService.GetByIdAsyncVm(id));
    }

    [HttpPost("add-book")]
    public async Task<ActionResult> AddBook(BookEditVm bookEditVm)
    {
        await bookService.AddAsync(bookEditVm);
        return Created();
    }

    [HttpDelete("delete-book/{id}")]
    public async Task<ActionResult> DeleteBook(Guid id)
    {
        await bookService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("update-book/{id}")]
    public async Task<ActionResult> UpdateBook(Guid id, BookEditVm bookEditVm)
    {
        await bookService.UpdateAsync(id, bookEditVm);
        return NoContent();
    }
}
