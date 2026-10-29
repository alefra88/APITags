using Microsoft.AspNetCore.Identity;
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
            /*tomo la llave secreta que tengo protegida en las variables de entorno de mi Lambda
            y configuro las credenciales de firma utilizando el algoritmo de encriptación fuerte HMAC-SHA256.*/
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? string.Empty));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            /*Agrego los claims necesarios: el nombre del usuario (Sub), su rol en el sistema 
            (para controlar a qué pantallas de Angular o rutas de la API tiene acceso después) 
            y un identificador único global para este token en específico (JTI).*/
                new Claim(JwtRegisteredClaimNames.Sub, usuarioDb.Username),
                new Claim("rol", usuarioDb.Rol), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
/*Configuro el emisor (Issuer), la audiencia (Audience) y le pongo un tiempo de vida 
de 15 minutos para minimizar riesgos de seguridad en caso de que alguien intercepte el token.*/
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
/*retorno al final es el JWT convertido en texto plano para que Angular lo reciba y lo almacene.*/
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
