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

        public async Task<NominaResponseDTO> CalcularYGenerarNominaAsync(string empleadoCedula, decimal salarioBase)
        {
            // 1. Obtener datos del Empleado (solo para validación)
            var empleado = await _empleadoRepository.GetByCedulaAsync(empleadoCedula);
            if (empleado == null)
            {
                throw new KeyNotFoundException($"Empleado con cédula {empleadoCedula} no encontrado.");
            }

            // 2. Lógica de Cálculo (Ajustada a propiedades existentes y salario externo)

            // ** Cálculo de Deducciones **
            decimal deduccionImpuesto = salarioBase * 0.10M;
            decimal deduccionINSS = salarioBase * 0.07M;
            decimal totalDeducciones = deduccionImpuesto + deduccionINSS;

            // ** Cálculo de Devengado (Salario Bruto) **
            decimal devengadoTotal = salarioBase;

            // ** Cálculo de Salario Neto **
            decimal salarioNetoCalculado = devengadoTotal - totalDeducciones;

            // 3. Crear el Modelo de Nomina (Modelo de BD)
            var nuevaNomina = new Nomina
            {
                EmpleadoCedula = empleado.Cedula,

                
                Salario = salarioBase, // Use la propiedad Salario como el Salario Base
                Devengado = devengadoTotal, // Salario Bruto
                INSSLaboral = deduccionINSS,
                MontoDeducciones = totalDeducciones, // Total de deducciones
                SalarioNeto = salarioNetoCalculado,

                // Propiedades requeridas por la BD que deben inicializarse
                HorasExtras = 0,
                MontoDeHorasExtras = 0,
                Inasistencia = 0

            };

            // 4. Persistir
            await _nominaRepository.AddAsync(nuevaNomina);

            // 5. Mapear a DTO de Respuesta
            return _mapper.Map<NominaResponseDTO>(nuevaNomina);
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
