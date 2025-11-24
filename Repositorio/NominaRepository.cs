using Microsoft.EntityFrameworkCore;
using Sistema_Web_de_Nominas.Data;
using Sistema_Web_de_Nominas.Models;

namespace Sistema_Web_de_Nominas.Repositorio
{
    public class NominaRepository(NominaDbContext context) : INominaRepository
    {
        private readonly NominaDbContext _context = context;
        public async Task AddAsync(Nomina nomina)
        {
            await _context.Nomina.AddAsync(nomina);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int codigoId)
        {
            var nomina = await GetByIdAsync(codigoId);
            if (nomina != null)
            {
                _context.Nomina.Remove(nomina);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Nomina>> GetAllAsync()
        {
            return await _context.Nomina.ToArrayAsync();
        }

        public async Task<Nomina?> GetByIdAsync(int codigoId)
        {
            return await _context.Nomina.FindAsync(codigoId);
        }

        public async Task<IEnumerable<Nomina>> GetNominasByEmpleadoCedulaAsync(string empleadoCedula)
        {
            return await _context.Nomina
                .Where(n => n.EmpleadoCedula == empleadoCedula)
                .ToListAsync();
        }

        public async Task UpdateAsync(Nomina nomina)
        {
            _context.Nomina.Update(nomina);
           
            await _context.SaveChangesAsync();
        }
    }
}
