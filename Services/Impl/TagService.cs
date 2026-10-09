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
    }
}
