using JwtTokenMicroServiceProvider.Models;
using JwtTokenMicroServiceProvider.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace JwtTokenMicroServiceProvider.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AuthController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    /// <summary>
    /// Skapar en JWT-token för en användare.
    /// </summary>
    /// <param name="dto">Användarinfo</param>
    /// <returns>JWT-token</returns>
    [HttpPost("token")]
    [SwaggerOperation(Summary = "Skapar en JWT-token baserat på användar-ID och roll.")]
    public IActionResult CreateToken([FromBody] CreateTokenDto dto)
    {
        var token = _tokenService.GenerateToken(dto.UserId, dto.Role);
        return Ok(new { token });
    }

    /// <summary>
    /// Verifierar en existerande JWT-token.
    /// </summary>
    /// <param name="dto">Token att verifiera</param>
    /// <returns>Valideringsresultat</returns>
    [HttpPost("verify")]
    [SwaggerOperation(Summary = "Verifierar en JWT-token och returnerar information.")]
    public IActionResult VerifyToken([FromBody] TokenVerificationDto dto)
    {
        var result = _tokenService.ValidateToken(dto.Token);
        if (!result.ValidToken)
            return BadRequest(new { result.ValidToken, result.Error });

        return Ok(new { result.ValidToken, result.UserId, result.Role });
    }
}
