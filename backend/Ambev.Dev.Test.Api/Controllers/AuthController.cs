using Ambev.Dev.Test.Domain.Contracts.Services;
using Ambev.Dev.Test.Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.Dev.Test.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("signin")]
    [ProducesResponseType<SignInResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SignIn([FromBody] SignInCredentials credentials, CancellationToken cancellationToken)
    {
        var result = await authService.SignIn(credentials, cancellationToken);
        return Ok(result);
    }
}
