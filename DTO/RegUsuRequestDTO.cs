namespace TagAPI.DTO
{
    public class RegUsuRequestDTO
    {
        public string Usuario { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
        public string Rol {  get; set; }    = "Cliente";
    }
}
