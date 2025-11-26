using Sistema_Web_de_Nominas.Dto;

namespace Sistema_Web_de_Nominas.Services
{
    public interface INominaService
    {
        Task<IEnumerable<NominaResponseDTO>> GetAllNominasAsync();
        Task<NominaResponseDTO?> GetNominaByIdAsync(int codigoId);
        Task<NominaResponseDTO> AddNominaAsync(NominaRequestDTO nominaDto);
        Task<NominaResponseDTO> UpdateNominaAsync(int codigoId, NominaRequestDTO nominaDto);
        Task DeleteNominaAsync(int codigoId);

        Task<NominaResponseDTO> CalcularYGenerarNominaAsync(NominaRequestDTO dto);
        Task<IEnumerable<NominaResponseDTO>> GetNominasPorEmpleadoAsync(string empleadoCedula);
    }
}
