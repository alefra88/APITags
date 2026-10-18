using Microsoft.AspNetCore.Mvc;
using TagAPI.DTO;
using TagAPI.Services.Interfaces;

namespace TagAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var token = await _authService.AutenticarAsync(request);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });
            }

            return Ok(new { token });
        }
    }
}