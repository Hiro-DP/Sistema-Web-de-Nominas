using Sistema_Web_de_Nominas.Models;

namespace Sistema_Web_de_Nominas.Repositorio
{
    public interface IEmpleadoRepository
    {
        Task<IEnumerable<Empleado>> GetAllAsync();
        Task<Empleado?> GetByCedulaAsync(string cedula);
        Task AddAsync(Empleado empleado);
        Task UpdateAsync(Empleado empleado); 
        Task DeleteAsync(string cedula);

        
        Task<IEnumerable<Empleado>> GetEmpleadosByCargoAsync(string cargo);

        
        Task SaveChangesAsync();
    }
}
