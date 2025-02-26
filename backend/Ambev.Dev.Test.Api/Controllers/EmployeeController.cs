using Microsoft.AspNetCore.Mvc;

namespace Ambev.Dev.Test.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(new { EmployeeId = 1 });
    }
}
