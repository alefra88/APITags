using TagAPI.Models;

namespace TagAPI.Repositories.Interfaces
{
        public interface ITagRepo
        {
            Task<Tag> CrearTagAsync(Tag tag);
        }
}
