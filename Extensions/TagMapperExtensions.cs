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
    }
}
