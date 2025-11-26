using Microsoft.EntityFrameworkCore;
using Sistema_Web_de_Nominas.Data;
using Sistema_Web_de_Nominas.Models;
using Sistema_Web_de_Nominas.Repositorio;
using System.Threading;

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

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuario.ToListAsync();
        }

        public async Task<Usuario?> GetUsuarioById(int id)
        {
            return await _context.Usuario.FindAsync(id);
        }
        public async Task<Usuario> UpdateUsuario(Usuario usuario)
        {
            _context.Usuario.Update(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}
