using System.Threading.Tasks;
using Sistema_Web_de_Nominas.Dto;
using Sistema_Web_de_Nominas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace Sistema_Web_de_Nominas.Controllers
{
    [Authorize(Roles = "Admin,Usuario")]
    public class NominaController(INominaService nominaService, IEmpleadoService empleadoService) : Controller
    {
        private readonly INominaService _nominaService = nominaService;
        private readonly IEmpleadoService _empleadoService = empleadoService;

        public async Task<IActionResult> Index()
        {
            var nominas = await _nominaService.GetAllNominasAsync();
            return View(nominas);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int codigoId)
        {
            var nomina = await _nominaService.GetNominaByIdAsync(codigoId);
            if (nomina == null)
            {
                return NotFound($"Nómina con ID {codigoId} no encontrada.");
            }
            return View(nomina);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Empleados = await _empleadoService.GetAllEmpleadosAsync();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] NominaRequestDTO nominaDto)
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
                var nominaCreada = await _nominaService.AddNominaAsync(nominaDto);
                return Ok(nominaCreada);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al crear la nómina", detalle = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CalculateAndGenerate([FromBody] NominaRequestDTO dto)
        {
            if (string.IsNullOrEmpty(dto.EmpleadoCedula) || dto.Salario <= 0)
            {
                return BadRequest(new { error = "La cédula del empleado y el salario base son obligatorios." });
            }

            try
            {
                var nominaGenerada = await _nominaService.CalcularYGenerarNominaAsync(dto); // <-- Cambiado

                return Ok(nominaGenerada);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al calcular y generar la nómina", detalle = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int codigoId)
        {
            var nomina = await _nominaService.GetNominaByIdAsync(codigoId);
            if (nomina == null)
            {
                return NotFound($"Nómina con ID {codigoId} no encontrada.");
            }
            var editDto = new NominaRequestDTO
            {
                Salario = nomina.Salario,
                HorasExtras = nomina.HorasExtras,
                Inasistencia = nomina.Inasistencia,
                EmpleadoCedula = nomina.EmpleadoCedula
            };
            ViewBag.Empleados = await _empleadoService.GetAllEmpleadosAsync();
            return View(editDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int codigoId, [FromBody] NominaRequestDTO nominaDto)
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
                var nominaActualizada = await _nominaService.UpdateNominaAsync(codigoId, nominaDto);
                return Ok(nominaActualizada);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al actualizar la nómina", detalle = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int codigoId)
        {
            try
            {
                await _nominaService.DeleteNominaAsync(codigoId);
                return Ok(new { success = true, message = $"Nómina con ID {codigoId} eliminada correctamente." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Error al eliminar la nómina", detalle = ex.Message });
            }
        }
    }
}