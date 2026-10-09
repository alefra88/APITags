using TagAPI.DTO;
using TagAPI.Models;

namespace TagAPI.Services.Interfaces
{
    public interface ITagService
    {
        Task<Tag> RegistrarNuevoTagAsync(TagCreateRequestDTO request);
    }
}
