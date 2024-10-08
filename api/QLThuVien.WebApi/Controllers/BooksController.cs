﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [FromQuery] string category = "",
        [FromQuery] string order = "Title"
        )
    {
        return Ok(await bookService.QueryAsyncVm
            (
                pageIndex,
                pageSize,
                u => u.Title.Contains(title) && (category.Length == 0 || u.Category.Name.Equals(category)),
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

    [HttpPut("give-rating")]
    [EndpointDescription("Return new avg rating of same book")]
    public async Task<ActionResult<double?>> GiveRating(RatingVm ratingVm)
    {
        return Ok(await bookService.GiveRating(ratingVm));
    }

    [HttpGet("get-remaining-count/{id}")]
    public async Task<ActionResult<int>> GetRemainingCount(Guid id)
    {
        return Ok(await bookService.GetRemainingCountAsync(id));
    }
}
