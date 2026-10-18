using TagAPI.DTO;
using TagAPI.Models;
using TagAPI.Repositories.Interfaces;
using TagAPI.Services.Interfaces;
using Tag = TagAPI.Models.Tag;

namespace TagAPI.Services.Impl
{
    public class TagService : ITagService
    {
        private readonly ITagRepo _tagRepo;

        public TagService(ITagRepo tagRepo)
        {
            _tagRepo = tagRepo;
        }

        public async Task<Tag> RegistrarNuevoTagAsync(TagCreateRequestDTO request)
        {
            // Generamos un número de TAG único automáticamente en el servidor
            string numeroTagGenerado = $"TAG-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

            var tag = new Tag
            {
                NumeroTag = numeroTagGenerado, 
                PlacaVehiculo = request.PlacaVehiculo, // Único dato que viene del cliente
                Saldo = 0.0m,
                Estado = "ACTIVO",
                FechaRegistro = DateTime.UtcNow
            };

            return await _tagRepo.CrearTagAsync(tag);
        }

        public async Task<IEnumerable<Tag>> ObtenerTodosAsync()
        {
            return await _tagRepo.ObtenerTagsAsync();
        }

        public async Task<Tag?> ObtenerPorIdAsync(string numeroTag)
        {
            return await _tagRepo.ObtenerTagPorIdAsync(numeroTag);
        }

        public async Task<Tag?> ActualizarTagAsync(string numeroTag, TagUpdateRequestDTO request)
        {
            var tagExistente = await _tagRepo.ObtenerTagPorIdAsync(numeroTag);
            if (tagExistente == null) return null;

            // Actualizamos solo los campos permitidos
            tagExistente.PlacaVehiculo = request.PlacaVehiculo;
            // No modificamos NumeroTag ni FechaRegistro

            await _tagRepo.ActualizarTagAsync(tagExistente);
            return tagExistente;
        }

        public async Task<bool> EliminarTagAsync(string numeroTag)
        {
            var tagExistente = await _tagRepo.ObtenerTagPorIdAsync(numeroTag);
            if (tagExistente == null) return false;

            await _tagRepo.EliminarTagAsync(numeroTag);
            return true;
        }
    }
}
