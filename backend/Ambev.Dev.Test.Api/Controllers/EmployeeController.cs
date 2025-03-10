using Ambev.Dev.Test.Domain.Contracts.Services;
using Ambev.Dev.Test.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.Dev.Test.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
    [HttpGet("search")]
    [ProducesResponseType<List<EmployeeModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Search([FromQuery] string firstName, [FromQuery] string lastName, CancellationToken cancellationToken)
    {
        var employee = await employeeService.Search(firstName, lastName, cancellationToken);
        return Ok(employee);
    }

    [HttpGet("all")]
    [ProducesResponseType<List<EmployeeSimpleModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var employee = await employeeService.GetAll(cancellationToken);
        return Ok(employee);
    }

    [HttpGet("{id}")]
    [ProducesResponseType<EmployeeModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetOne(int id, CancellationToken cancellationToken)
    {
        var employee = await employeeService.GetById(id, cancellationToken);
        return Ok(employee);
    }

    [HttpPost]
    [ProducesResponseType<object>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] EmployeeManageModel model, CancellationToken cancellationToken)
    {
        var employeeId = await employeeService.Create(model, cancellationToken);
        return Ok(new { employeeId });
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update([FromBody] EmployeeManageModel model, CancellationToken cancellationToken)
    {
        await employeeService.Update(model, cancellationToken);
        return Ok();
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
