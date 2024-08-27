using Microsoft.AspNetCore.Mvc;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Interfaces;

namespace QLThuVien.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BorrowController : ControllerBase
{
    private readonly IBorrowService _borrowService;
    private readonly ILogger<BorrowController> _logger;

    public BorrowController(IBorrowService borrowService, ILogger<BorrowController> logger)
    {
        _borrowService = borrowService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var borrows = await _borrowService.GetAllAsync();
        return Ok(borrows);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var borrow = await _borrowService.GetByIdAsync(id);
        if (borrow == null)
        {
            _logger.LogWarning($"Borrow with id {id} not found.");
            return NotFound();
        }
        return Ok(borrow);
    }

    [HttpPost]
    public async Task<IActionResult> Add(Borrow borrow)
    {
        try
        {
            _borrowService.AddAsync(borrow);
            return CreatedAtAction(nameof(GetById), new { id = borrow.Id }, borrow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding borrow.");
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, Borrow borrow)
    {
        if (id != borrow.Id)
        {
            _logger.LogWarning("Mismatch between URL id and borrow id.");
            return BadRequest("Id mismatch");
        }

        try
        {
            _borrowService.UpdateAsync(borrow);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating borrow.");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
             _borrowService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting borrow.");
            return BadRequest(ex.Message);
        }
    }
}