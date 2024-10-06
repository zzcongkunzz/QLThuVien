using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLThuVien.Business.ML;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;

namespace QLThuVien.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RecommenderController 
    (IRecommenderService recommenderService)
    : ControllerBase
{
    [HttpGet("get-recommended-books/{userId}")]
    public async Task<ActionResult<IEnumerable<BookVm>>> GetRecommededBooks(
        Guid userId, [FromQuery] int count = 4)
    {
        try
        {
            return Ok(await recommenderService.GetRecommendedBooksFromCandidates(userId, count)); 
        } catch
        {
            return BadRequest("Model may not be correctly initialized, try training it first");
        }
    }

    [HttpGet("get-similar-books/{bookId}")]
    public async Task<ActionResult<IEnumerable<BookVm>>> GetSimilarBooks(
        Guid bookId, [FromQuery] int count = 4)
    {
        try
        {
            return Ok(await recommenderService.GetSimilarBookVms(bookId, count));
        }
        catch
        {
            return BadRequest("Model may not be correctly initialized, try training it first");
        }
    }

    [HttpGet("get-recommended-books-baseline/{userId}")]
    public async Task<ActionResult<IEnumerable<BookVm>>> GetRecommededBooksBaseline(
        Guid userId, [FromQuery] int count = 4)
    {
        return Ok(await recommenderService.GetRecommendedBooksBaselineVm(userId, count));
    }

    [HttpGet("get-highest-rated-books")]
    public async Task<ActionResult<IEnumerable<BookVm>>> GetHigestRatedBooks([FromQuery] int count = 4)
    {
        return Ok(await recommenderService.GetHighestRatedBooksVm(count));
    }


    [HttpPut("train")]
    public async Task<ActionResult<TrainResult>> TrainModel(
        IServiceProvider services,
        ModelManager modelManager,
        [FromQuery] int synthSize = 2)
    {
        return Ok(await modelManager.Train(synthSize, services));
    }

    [HttpPut("load")]
    public ActionResult<long> LoadModel(
        IServiceProvider services,
        ModelManager modelManager)
    {
        return Ok(modelManager.LoadModelFromDisk());
    }
}
