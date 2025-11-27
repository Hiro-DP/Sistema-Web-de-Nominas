using AutoMapper;
using Sistema_Web_de_Nominas.Dto;
using Sistema_Web_de_Nominas.Models;
using Sistema_Web_de_Nominas.Repositorio;

namespace Sistema_Web_de_Nominas.Services
{
    public class NominaService(INominaRepository nominaRepository, IEmpleadoRepository empleadoRepository, IMapper mapper) : INominaService
    {
        private readonly INominaRepository _nominaRepository = nominaRepository;
        private readonly IEmpleadoRepository _empleadoRepository = empleadoRepository;
        private readonly IMapper _mapper = mapper;

        private Nomina RecalcularNomina(Nomina nominaModel)
        {
            const decimal TASA_INSS = 0.07m; 
            const int DIAS_LABORABLES_MES = 30; 

            decimal salarioPorHora = nominaModel.Salario / DIAS_LABORABLES_MES / 8;
            decimal valorHoraExtra = salarioPorHora * 1.5m;
            nominaModel.MontoDeHorasExtras = valorHoraExtra * (decimal)nominaModel.HorasExtras;

            nominaModel.Devengado = nominaModel.Salario + nominaModel.MontoDeHorasExtras;

            nominaModel.INSSLaboral = nominaModel.Devengado * TASA_INSS;

            decimal valorDia = nominaModel.Salario / DIAS_LABORABLES_MES;
            decimal deduccionInasistencia = valorDia * nominaModel.Inasistencia;

            nominaModel.MontoDeducciones = nominaModel.INSSLaboral + deduccionInasistencia;

            nominaModel.SalarioNeto = nominaModel.Devengado - nominaModel.MontoDeducciones;

            return nominaModel;
        }

        public async Task<NominaResponseDTO> AddNominaAsync(NominaRequestDTO nominaDto)
        {
            var nomina = _mapper.Map<Nomina>(nominaDto);

            var empleado = await _empleadoRepository.GetByCedulaAsync(nomina.EmpleadoCedula);
            if (empleado == null)
            {
                throw new KeyNotFoundException($"No se puede registrar la nómina: Empleado con cédula {nomina.EmpleadoCedula} no encontrado.");
            }

            await _nominaRepository.AddAsync(nomina);

            return _mapper.Map<NominaResponseDTO>(nomina);
        }

        public async Task<NominaResponseDTO> CalcularYGenerarNominaAsync(NominaRequestDTO dto)
        {
            var nominaModel = _mapper.Map<Nomina>(dto);

            // APLICAR EL CÁLCULO
            nominaModel = RecalcularNomina(nominaModel);

            // Guardar en la base de datos
            await _nominaRepository.AddAsync(nominaModel);

            // Devolver el resultado
            return _mapper.Map<NominaResponseDTO>(nominaModel);
        }

        public async Task DeleteNominaAsync(int codigoId)
        {
            var existeNomina = await _nominaRepository.GetByIdAsync(codigoId);
            if (existeNomina != null)
            {
                await _nominaRepository.DeleteAsync(codigoId);
            }
            else
            {
                throw new KeyNotFoundException($"La nómina con ID {codigoId} no existe.");
            }
        }

        public async Task<IEnumerable<NominaResponseDTO>> GetAllNominasAsync()
        {
            var nominas = await _nominaRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<NominaResponseDTO>>(nominas);
        }

        public async Task<NominaResponseDTO?> GetNominaByIdAsync(int codigoId)
        {
            var nomina = await _nominaRepository.GetByIdAsync(codigoId);
            return _mapper.Map<NominaResponseDTO?>(nomina);
        }

        public async Task<IEnumerable<NominaResponseDTO>> GetNominasPorEmpleadoAsync(string empleadoCedula)
        {
            var nominas = await _nominaRepository.GetNominasByEmpleadoCedulaAsync(empleadoCedula);
            return _mapper.Map<IEnumerable<NominaResponseDTO>>(nominas);
        }

        public async Task<NominaResponseDTO> UpdateNominaAsync(int codigoId, NominaRequestDTO nominaDto)
        {
            var nominaExistente = await _nominaRepository.GetByIdAsync(codigoId);
            if (nominaExistente == null)
            {
                throw new KeyNotFoundException($"Nómina con ID {codigoId} no encontrada para actualizar.");
            }

            _mapper.Map(nominaDto, nominaExistente);
            nominaExistente.CodigoId = codigoId;

            nominaExistente = RecalcularNomina(nominaExistente);

            await _nominaRepository.UpdateAsync(nominaExistente);

            return _mapper.Map<NominaResponseDTO>(nominaExistente);
        }
    }
}
