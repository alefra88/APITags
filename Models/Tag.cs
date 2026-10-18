namespace TagAPI.Models
{
    public class Tag
    {
        //public int Id { get; set; }
        public string NumeroTag { get; set; }
        public string PlacaVehiculo { get; set; }
        public decimal Saldo { get; set; }
        public string Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
