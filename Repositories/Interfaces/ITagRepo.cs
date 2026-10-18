using TagAPI.Models;

namespace TagAPI.Repositories.Interfaces
{
    public interface ITagRepo
    {
        Task<Tag> CrearTagAsync(Tag tag);
        Task<IEnumerable<Tag>> ObtenerTagsAsync();
        Task<Tag?> ObtenerTagPorIdAsync(string numeroTag);
        Task ActualizarTagAsync(Tag tag);
        Task EliminarTagAsync(string numeroTag);
    }
}
