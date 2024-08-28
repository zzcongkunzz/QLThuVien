using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QLThuVien.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PenaltyController : ControllerBase
{
    
}