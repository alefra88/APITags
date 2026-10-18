using Amazon.DynamoDBv2.DataModel;

namespace TagAPI.Models
{
    [DynamoDBTable("UsuariosTelepeaje")]
    public class Usuario
    {
        [DynamoDBHashKey] // Clave primaria
        public string Username { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string Password { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string Rol { get; set; } = "Cliente";
    }
}