using Sistema_Web_de_Nominas.Models;

namespace Sistema_Web_de_Nominas.Repositorio
{
    public interface INominaRepository
    {
        Task<IEnumerable<Nomina>> GetAllAsync();
        Task<Nomina?> GetByIdAsync(int codigoId);
        Task AddAsync(Nomina nomina);
        Task UpdateAsync(Nomina nomina); 
        Task DeleteAsync(int codigoId);
        
        Task<IEnumerable<Nomina>> GetNominasByEmpleadoCedulaAsync(string empleadoCedula);
    }
}
