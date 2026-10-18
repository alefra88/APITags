using TagAPI.DTO;

namespace TagAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string?> AutenticarAsync(LoginRequestDTO request);
    }
}
