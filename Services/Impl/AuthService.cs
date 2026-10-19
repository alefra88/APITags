using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TagAPI.DTO;
using TagAPI.Repositories.Interfaces;
using TagAPI.Services.Interfaces;

namespace TagAPI.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUsuarioRepo _usuarioRepo;

        public AuthService(IConfiguration config, IUsuarioRepo usuarioRepo)
        {
            _config = config;
            _usuarioRepo = usuarioRepo;
        }

        public async Task<string?> AutenticarAsync(LoginRequestDTO request)
        {
            // 1. Buscar el usuario en DynamoDB
            var usuarioDb = await _usuarioRepo.ObtenerPorUsernameAsync(request.Usuario);

            // 2. Validar que exista y la contraseña coincida
            if (usuarioDb == null || usuarioDb.Password != request.Password)
            {
                return null; // Rechazado
            }

            // 3. Generar token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? string.Empty));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioDb.Username),
                new Claim("rol", usuarioDb.Rol), // Añadimos su rol al token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}