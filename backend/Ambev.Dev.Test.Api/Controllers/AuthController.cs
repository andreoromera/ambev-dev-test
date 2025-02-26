using Ambev.Dev.Test.Domain.Contracts.Services;
using Ambev.Dev.Test.Domain.Security;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.Dev.Test.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("signin")]
    [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SignIn([FromBody] SignInCredentials credentials, CancellationToken cancellationToken)
    {
        var result = await authService.SignIn(credentials, cancellationToken);
        return Ok(result);
    }
}
