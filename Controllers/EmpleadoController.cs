using System.Threading.Tasks;
using Sistema_Web_de_Nominas.Dto;
using Sistema_Web_de_Nominas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace Sistema_Web_de_Nominas.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmpleadoController(IEmpleadoService empleadoService) : Controller
    {
        private readonly IEmpleadoService _empleadoService = empleadoService;

        public async Task<IActionResult> Index()
        {
            var empleados = await _empleadoService.GetAllEmpleadosAsync();
            return View(empleados);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string cedula)
        {
            var empleado = await _empleadoService.GetEmpleadoByCedulaAsync(cedula);
            if (empleado == null)
            {
                return NotFound($"Empleado con cédula {cedula} no encontrado.");
            }
            return View(empleado);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmpleadoRequestDTO empleadoDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { errors });
            }

            try
            {
                var empleadoCreado = await _empleadoService.AddEmpleadoAsync(empleadoDto);
                return Ok(empleadoCreado);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al crear el empleado", detalle = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string cedula)
        {
            var empleado = await _empleadoService.GetEmpleadoByCedulaAsync(cedula);
            if (empleado == null)
            {
                return NotFound($"Empleado con cédula {cedula} no encontrado.");
            }
            var editDto = new EmpleadoRequestDTO
            {
                Cedula = empleado.Cedula,
                NombreCompleto = empleado.NombreCompleto,
                Telefono = empleado.Telefono,
                Sexo = empleado.Sexo,
                Cargo = empleado.Cargo
            };
            return View(editDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string cedula, [FromBody] EmpleadoRequestDTO empleadoDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { errors });
            }

            try
            {
                var empleadoActualizado = await _empleadoService.UpdateEmpleadoAsync(cedula, empleadoDto);
                return Ok(empleadoActualizado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al actualizar el empleado", detalle = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string cedula)
        {
            try
            {
                await _empleadoService.DeleteEmpleadoAsync(cedula);
                return Ok(new { success = true, message = $"Empleado con cédula {cedula} eliminado correctamente." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error al eliminar el empleado", detalle = ex.Message });
            }
        }
    }
}