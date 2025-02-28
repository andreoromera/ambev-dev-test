using Ambev.Dev.Test.Domain.Contracts.Services;
using Ambev.Dev.Test.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.Dev.Test.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var employee = await employeeService.GetById(id, cancellationToken);
        return Ok(employee);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeModel model, CancellationToken cancellationToken)
    {
        var employeeId = await employeeService.Create(model, cancellationToken);
        return Ok(new { employeeId });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await employeeService.Delete(id, cancellationToken);
        return Ok();
    }
}
