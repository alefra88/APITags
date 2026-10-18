using System.ComponentModel.DataAnnotations;

namespace TagAPI.DTO
{
    public class TagUpdateRequestDTO
    {
        [Required(ErrorMessage = "La placa del vehículo es obligatoria.")]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "La placa debe tener entre 5 y 10 caracteres.")]
        [RegularExpression(@"^(?:[A-HJ-NP-Z]{3}-\d{4}|[A-HJ-NP-Z]{3}-\d{3}-[A-HJ-NP-Z]|[A-HJ-NP-Z]{2}-\d{4,5}|[A-HJ-NP-Z]-\d{3}-[A-HJ-NP-Z]{3})$", ErrorMessage = "Formato de placa inválido. Particular Tradicional: ABC-1234, Particular Nuevo:AAA-000-A, Transporte Público 1: AB-12345 o AB-1234, Transporte Público 2: A-000-AAA")]
        public string PlacaVehiculo { get; set; }

    }
}
