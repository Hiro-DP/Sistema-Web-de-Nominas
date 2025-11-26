using Sistema_Web_de_Nominas.Models;
using System.Threading;

namespace Sistema_Web_de_Nominas.Repositorio
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetUserByUserName(string nombreUsuario);
        Task<Usuario?> GetUserByEmail(string correo);
        Task<Usuario?> GetUsuarioById(int id);
        Task<Usuario> UpdateUsuario(Usuario usuario);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario> AddAsync(Usuario usuario);
        bool ValidatePassword(Usuario usuario, string contra);
        Task SaveAsync();
        Task<Usuario?> GetUserByRefreshTokenAsync(string refreshToken);
    }
}
