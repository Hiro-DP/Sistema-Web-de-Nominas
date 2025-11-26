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

            // --- REGLAS DE CÁLCULO (Ejemplo) ---
            const decimal TASA_INSS = 0.07m; // 7% de INSS Laboral
            const int DIAS_LABORABLES_MES = 30; // Para calcular el valor por día

            // 1. Cálculo de Monto por Horas Extras
            // Suponiendo que el valor de la hora extra es 1.5 veces la hora normal
            decimal salarioPorHora = nominaModel.Salario / DIAS_LABORABLES_MES / 8;
            decimal valorHoraExtra = salarioPorHora * 1.5m;
            nominaModel.MontoDeHorasExtras = valorHoraExtra * (decimal)nominaModel.HorasExtras;

            // 2. Cálculo del Devengado (Salario Base + Horas Extras)
            nominaModel.Devengado = nominaModel.Salario + nominaModel.MontoDeHorasExtras;

            // 3. Cálculo de Deducciones
            // a) Deducción por INSS sobre el devengado
            nominaModel.INSSLaboral = nominaModel.Devengado * TASA_INSS;

            // b) Deducción por Inasistencia
            decimal valorDia = nominaModel.Salario / DIAS_LABORABLES_MES;
            decimal deduccionInasistencia = valorDia * nominaModel.Inasistencia;

            // c) Suma de todas las deducciones
            nominaModel.MontoDeducciones = nominaModel.INSSLaboral + deduccionInasistencia;

            // 4. Cálculo del Salario Neto
            nominaModel.SalarioNeto = nominaModel.Devengado - nominaModel.MontoDeducciones;

            // --- FIN DE LAS REGLAS DE CÁLCULO ---

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

            await _nominaRepository.UpdateAsync(nominaExistente);

            return _mapper.Map<NominaResponseDTO>(nominaExistente);
        }
    }
}
