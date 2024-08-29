using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLThuVien.Business.Services.Implementations;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;

namespace QLThuVien.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecommenderController 
    (IRecommenderService recommenderService)
    : ControllerBase
{
    [HttpGet("get-highest-rated-books")]
    public async Task<ActionResult<IEnumerable<BookVm>>> GetHigestRatedBooks()
    {
        return Ok(await recommenderService.GetHighestRatedBooks());
    }

    [HttpGet("get-recommended-books/{userId}")]
    public async Task<ActionResult<IEnumerable<BookVm>>> GetRecommededBooks(Guid userId)
    {
        return Ok(await recommenderService.GetRecommendedBooks(userId));
    }
}
