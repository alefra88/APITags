using TagAPI.DTO;

namespace TagAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegistrarUsuariosAsync(RegUsuRequestDTO request);
    }
}
