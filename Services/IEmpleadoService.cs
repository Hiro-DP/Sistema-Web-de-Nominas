using Sistema_Web_de_Nominas.Dto;

namespace Sistema_Web_de_Nominas.Services
{
    public interface IEmpleadoService
    {
        Task<IEnumerable<EmpleadoResponseDTO>> GetAllEmpleadosAsync();
        Task<EmpleadoResponseDTO?> GetEmpleadoByCedulaAsync(string cedula);
        Task<EmpleadoResponseDTO> AddEmpleadoAsync(EmpleadoRequestDTO empleadoDto);
        Task<EmpleadoResponseDTO> UpdateEmpleadoAsync(string cedula, EmpleadoRequestDTO empleadoDto);
        Task DeleteEmpleadoAsync(string cedula);
        Task<IEnumerable<EmpleadoResponseDTO>> GetEmpleadosByCargoAsync(string cargo);
    }
}
