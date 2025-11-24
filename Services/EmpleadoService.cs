using AutoMapper;
using Sistema_Web_de_Nominas.Dto;
using Sistema_Web_de_Nominas.Models;
using Sistema_Web_de_Nominas.Repositorio;

namespace Sistema_Web_de_Nominas.Services
{
    public class EmpleadoService(IEmpleadoRepository empleadoRepository, IMapper mapper) : IEmpleadoService
    {
        private readonly IEmpleadoRepository _empleadoRepository = empleadoRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<EmpleadoResponseDTO> AddEmpleadoAsync(EmpleadoRequestDTO empleadoDto)
        {
           
            var empleado = _mapper.Map<Empleado>(empleadoDto);
            var existe = await _empleadoRepository.GetByCedulaAsync(empleado.Cedula);
            if (existe != null)
            {
                throw new InvalidOperationException($"Ya existe un empleado con la cédula {empleado.Cedula}.");
            }
            await _empleadoRepository.AddAsync(empleado);
            return _mapper.Map<EmpleadoResponseDTO>(empleado);
        }

        public async Task DeleteEmpleadoAsync(string cedula)
        {
            var existeEmpleado = await _empleadoRepository.GetByCedulaAsync(cedula);

            if (existeEmpleado != null)
            {
                await _empleadoRepository.DeleteAsync(cedula);
            }
            else
            {
                throw new KeyNotFoundException($"El empleado con cédula {cedula} no existe.");
            }
        }

        public async Task<IEnumerable<EmpleadoResponseDTO>> GetAllEmpleadosAsync()
        {
            var empleados = await _empleadoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmpleadoResponseDTO>>(empleados);
        }

        public async Task<EmpleadoResponseDTO?> GetEmpleadoByCedulaAsync(string cedula)
        {
            var empleado = await _empleadoRepository.GetByCedulaAsync(cedula);
            return _mapper.Map<EmpleadoResponseDTO?>(empleado);
        }

        public async Task<IEnumerable<EmpleadoResponseDTO>> GetEmpleadosByCargoAsync(string cargo)
        {
            var empleados = await _empleadoRepository.GetEmpleadosByCargoAsync(cargo);
            return _mapper.Map<IEnumerable<EmpleadoResponseDTO>>(empleados);
        }

        public async Task<EmpleadoResponseDTO> UpdateEmpleadoAsync(string cedula, EmpleadoRequestDTO empleadoDto)
        {
            var empleadoExistente = await _empleadoRepository.GetByCedulaAsync(cedula);
            if (empleadoExistente == null)
            {
                throw new KeyNotFoundException($"Empleado con cédula {cedula} no encontrado para actualizar.");
            }

            _mapper.Map(empleadoDto, empleadoExistente);
            empleadoExistente.Cedula = cedula;

            await _empleadoRepository.UpdateAsync(empleadoExistente);

            return _mapper.Map<EmpleadoResponseDTO>(empleadoExistente);
        }
    }
}
