using Microsoft.EntityFrameworkCore;
using Sistema_Web_de_Nominas.Data;
using Sistema_Web_de_Nominas.Models;
using Sistema_Web_de_Nominas.Repositorio;

namespace Sistema_Web_de_Nominas
{
    public class UsuarioRepository(NominaDbContext context) : IUsuarioRepository
    {
        private readonly NominaDbContext _context = context;

        public async Task<Usuario?> GetUserByUserName(string nombreUsuario)
        {
            return await _context.Usuario.Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
        }

        public async Task<Usuario?> GetUserByEmail(string correo)
        {
            return await _context.Usuario.Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Correo == correo);
        }

        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            var entry = await _context.Usuario.AddAsync(usuario);
            return entry.Entity;
        }

        public bool ValidatePassword(Usuario usuario, string contra)
        {
            return BCrypt.Net
                .BCrypt.EnhancedVerify(contra, usuario.Contra);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Usuario
                .FirstOrDefaultAsync(u => u.RecargaToken == refreshToken);
        }
    }
}
