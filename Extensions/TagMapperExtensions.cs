using TagAPI.DTO;
using TagAPI.Models;

namespace TagAPI.Extensions
{
    public static class TagMapperExtensions
    {
        public static TagCreatedResponseDTO ToCreatedResponse(this Tag tag)
        {
            return new TagCreatedResponseDTO
            {
                Mensaje = "TAG registrado de forma segura",
                DatosGenerados = new TagDetailDTO
                {
                    NumeroTag = tag.NumeroTag,
                    PlacaVehiculo = tag.PlacaVehiculo,
                    SaldoInicial = tag.Saldo,
                    EstadoActual = tag.Estado,
                    FechaDeRegistro = tag.FechaRegistro
                }
            };
        }
        // Mapeo para las respuestas GET (individual)
        public static TagDetailDTO ToDetailResponse(this Tag tag)
        {
            return new TagDetailDTO
            {
                NumeroTag = tag.NumeroTag,
                PlacaVehiculo = tag.PlacaVehiculo,
                SaldoInicial = tag.Saldo,
                EstadoActual = tag.Estado,
                FechaDeRegistro = tag.FechaRegistro
            };
        }

        // Mapeo para la respuesta GET general (lista)
        public static IEnumerable<TagDetailDTO> ToDetailResponseList(this IEnumerable<Tag> tags)
        {
            return tags.Select(tag => tag.ToDetailResponse());
        }
    }
}
