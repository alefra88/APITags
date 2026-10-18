using TagAPI.Models;

namespace TagAPI.Repositories.Interfaces
{
        public interface IUsuarioRepo
        {
            Task<Usuario?> ObtenerPorUsernameAsync(string username);
        }    
}
