using Ambev.Dev.Test.Domain.Auth;
using Ambev.Dev.Test.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.Dev.Test.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("signin")]
    [ProducesResponseType(typeof(SignInResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignIn([FromBody] Credentials credentials)
    {
        var result = await authService.SignIn(credentials);
        return Ok(result);
    }
}
