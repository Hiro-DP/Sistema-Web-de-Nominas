using Sistema_Web_de_Nominas.Services;
using Sistema_Web_de_Nominas.Dto; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sistema_Web_de_Nominas.Controllers
{
    [Authorize]
    public class NominaController(INominaService nominaService, IEmpleadoService empleadoService) : Controller
    {
        private readonly INominaService _nominaService = nominaService;
        private readonly IEmpleadoService _empleadoService = empleadoService;

        public class CalculoNominaViewModel
        {
            public NominaRequestDTO NominaData { get; set; } = new NominaRequestDTO();
            public IEnumerable<EmpleadoResponseDTO> Empleados { get; set; } = Enumerable.Empty<EmpleadoResponseDTO>();
        }

        public async Task<IActionResult> Index()
        {
            var nominas = await _nominaService.GetAllNominasAsync();
            return View(nominas); 
        }

        public async Task<IActionResult> Details(int codigoId)
        {
            var nomina = await _nominaService.GetNominaByIdAsync(codigoId);
            if (nomina == null)
            {
                TempData["Error"] = $"Nómina con ID {codigoId} no encontrada.";
                return RedirectToAction(nameof(Index));
            }
            return View(nomina);
        }

        public async Task<IActionResult> Calculate()
        {
            var empleados = await _empleadoService.GetAllEmpleadosAsync();
            var viewModel = new CalculoNominaViewModel
            {
                Empleados = empleados
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculate(NominaRequestDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _nominaService.CalcularYGenerarNominaAsync(dto.EmpleadoCedula, dto.Salario);
                    TempData["Success"] = $"Nómina generada para el empleado {dto.EmpleadoCedula}.";
                    return RedirectToAction(nameof(Details), new { codigoId = result.CodigoId });
                }
                catch (KeyNotFoundException ex)
                {
                    ModelState.AddModelError("EmpleadoCedula", ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocurrió un error inesperado al procesar la nómina.");
                }
            }

            var empleados = await _empleadoService.GetAllEmpleadosAsync();
            var viewModel = new CalculoNominaViewModel { NominaData = dto, Empleados = empleados };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int codigoId)
        {
            try
            {
                await _nominaService.DeleteNominaAsync(codigoId);
                TempData["Success"] = "Nómina eliminada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                TempData["Error"] = $"Error al eliminar: Nómina con ID {codigoId} no encontrada.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}