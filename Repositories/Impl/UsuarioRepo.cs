using Amazon.DynamoDBv2.DataModel;
using TagAPI.Models;
using TagAPI.Repositories.Interfaces;

namespace TagAPI.Repositories.Impl
{
    public class UsuarioRepo : IUsuarioRepo
    {
        private readonly IDynamoDBContext _context;

        public UsuarioRepo(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObtenerPorUsernameAsync(string username)
        {
            // Busca al usuario en la tabla usando su clave primaria
            return await _context.LoadAsync<Usuario>(username);
        }
    }
}
