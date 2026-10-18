using TagAPI.DTO;
using TagAPI.Models;

namespace TagAPI.Services.Interfaces
{
    public interface ITagService
    {
        Task<Tag> RegistrarNuevoTagAsync(TagCreateRequestDTO request);
        Task<IEnumerable<Tag>> ObtenerTodosAsync();
        Task<Tag?> ObtenerPorIdAsync(string numeroTag);
        Task<Tag?> ActualizarTagAsync(string numeroTag, TagUpdateRequestDTO request);
        Task<bool> EliminarTagAsync(string numeroTag);
    }
}
