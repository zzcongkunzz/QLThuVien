using Microsoft.AspNetCore.Mvc;
using QLThuVien.Business.Exceptions;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;

namespace QLThuVien.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class BorrowsController : ControllerBase
{
    private readonly IBorrowService _borrowService;
    private readonly ILogger<BorrowsController> _logger;

    public BorrowsController(IBorrowService borrowService, ILogger<BorrowsController> logger)
    {
        _borrowService = borrowService;
        _logger = logger;
    }

    [HttpGet("get-all-borrows")]
    public async Task<ActionResult<IEnumerable<BorrowVm>>> GetAllBorrows()
    {
        return Ok(await _borrowService.GetAllVm());
    }

    [HttpGet("get-borrow-filters")]
    public ActionResult<IEnumerable<string>> GetBorrowFilters()
    {
        return Ok(new string[] {"all", "returned", "non-returned", "penalties-unpaid"});
    }


    [HttpGet("query-borrows")]
    public async Task<ActionResult<IEnumerable<BorrowVm>>> QueryBorrows(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string filter = "all",
        [FromQuery] string order = "StartTime"
        )
    {
        return Ok(await _borrowService.GetAsyncVm
            (
                pageIndex,
                pageSize,
                filter switch
                {
                    "returned" => (borrow => borrow.ActualReturnTime!= null
                                        && borrow.ActualReturnTime < DateTime.Now),
                    "non-returned" => (borrow => borrow.ActualReturnTime == null
                                        || borrow.ActualReturnTime > DateTime.Now),
                    "penalties-unpaid" => (borrow => borrow.PaidPenalties < borrow.IssuedPenalties),
                    "all" => (_ => true),
                    _ => throw new BadRequestException("Unrecognized filter")
                },
                q => order switch
                {
                    _ => q.OrderBy(b => b.StartTime)
                }
            ));
    }

    [HttpGet("get-borrow-by-id/{id:guid}")]
    public async Task<ActionResult<BorrowVm>> GetBorrowById(Guid id)
    {
        return Ok(await _borrowService.GetByIdAsyncVm(id));
    }

    [HttpPost("add-borrow")]
    public async Task<ActionResult> AddBorrow(BorrowEditVm borrowEditVm)
    {
        await _borrowService.AddAsync(borrowEditVm);
        return Created();
    }

    [HttpPut("update-borrow/{id:guid}")]
    public async Task<ActionResult> UpdateBorrow(Guid id, BorrowEditVm borrowEditVm)
    {
        await _borrowService.UpdateAsync(id, borrowEditVm);
        return NoContent();
    }

    [HttpDelete("delete-borrow/{id:guid}")]
    public async Task<ActionResult> DeleteBorrow(Guid id)
    {
        await _borrowService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("return-borrow/{id:guid}")]
    public async Task<ActionResult> ReturnBorrow(Guid id)
    {
        return Ok(await _borrowService.ReturnBorrow(id));
    }

    [HttpPut("undo-return-borrow/{id:guid}")]
    public async Task<ActionResult> UndoReturnBorrow(Guid id)
    {
        return Ok(await _borrowService.ReturnBorrow(id));
    }
}