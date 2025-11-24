using Microsoft.EntityFrameworkCore;
using Sistema_Web_de_Nominas.Data;
using Sistema_Web_de_Nominas.Models;

namespace Sistema_Web_de_Nominas.Repositorio
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly NominaDbContext _context;
        public EmpleadoRepository(NominaDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Empleado empleado)
        {
            await _context.Empleado.AddAsync(empleado);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string cedula)
        {
            var empleado = await GetByCedulaAsync(cedula);
            if (empleado != null)
            {
                _context.Empleado.Remove(empleado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Empleado>> GetAllAsync()
        {
            return await _context.Empleado.ToListAsync();
        }

        public async Task<Empleado?> GetByCedulaAsync(string cedula)
        {
            return await _context.Empleado.FirstOrDefaultAsync(e => e.Cedula == cedula);
        }

        public async Task<IEnumerable<Empleado>> GetEmpleadosByCargoAsync(string cargo)
        {
            return await _context.Empleado
                .Where(e => e.Cargo == cargo)
                .ToListAsync();
        }

        public async Task UpdateAsync(Empleado empleado)
        {
            _context.Empleado.Update(empleado);
            await _context.SaveChangesAsync();
        }
    }
}
