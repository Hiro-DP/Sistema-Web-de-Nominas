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

        public Task AddAsync(Empleado empleado)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string cedula)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Empleado>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Empleado?> GetByCedulaAsync(string cedula)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Empleado>> GetEmpleadosByCargoAsync(string cargo)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Empleado empleado)
        {
            throw new NotImplementedException();
        }
    }
}
