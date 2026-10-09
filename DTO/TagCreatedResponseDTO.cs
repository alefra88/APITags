namespace TagAPI.DTO
{
    public class TagCreatedResponseDTO
    {
        public string Mensaje { get; set; }
        public TagDetailDTO DatosGenerados { get; set; }
    }

    public class TagDetailDTO
    {
        public string NumeroTag { get; set; }
        public string PlacaVehiculo { get; set; }
        public decimal SaldoInicial { get; set; }
        public string EstadoActual { get; set; }
        public DateTime FechaDeRegistro { get; set; }
    }
}
