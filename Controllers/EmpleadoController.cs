using Sistema_Web_de_Nominas.Dto;
using Sistema_Web_de_Nominas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sistema_Web_de_Nominas.Controllers
{
    [Authorize] 
    public class EmpleadoController(IEmpleadoService empleadoService) : Controller
    {
        private readonly IEmpleadoService _empleadoService = empleadoService;

        public async Task<IActionResult> Index()
        {
            var empleados = await _empleadoService.GetAllEmpleadosAsync();
            return View(empleados); 
        }

        public async Task<IActionResult> Details(string cedula)
        {
            var empleado = await _empleadoService.GetEmpleadoByCedulaAsync(cedula);
            if (empleado == null)
            {
                TempData["Error"] = $"Empleado con cédula {cedula} no encontrado.";
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        public IActionResult Create()
        {
            return View(new EmpleadoRequestDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmpleadoRequestDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _empleadoService.AddEmpleadoAsync(dto);
                    TempData["Success"] = "Empleado creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("Cedula", ex.Message);
                }
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(string cedula)
        {
            var empleado = await _empleadoService.GetEmpleadoByCedulaAsync(cedula);
            if (empleado == null)
            {
                TempData["Error"] = $"Empleado con cédula {cedula} no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var requestDto = new EmpleadoRequestDTO
            {
                Cedula = empleado.Cedula,
                NombreCompleto = empleado.NombreCompleto,
                Telefono = empleado.Telefono,
                Sexo = empleado.Sexo,
                Cargo = empleado.Cargo
            };
            return View(requestDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string cedula, EmpleadoRequestDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _empleadoService.UpdateEmpleadoAsync(cedula, dto);
                    TempData["Success"] = "Empleado actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (KeyNotFoundException)
                {
                    TempData["Error"] = $"Empleado con cédula {cedula} no encontrado.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(dto);
        }

        public async Task<IActionResult> Delete(string cedula)
        {
            var empleado = await _empleadoService.GetEmpleadoByCedulaAsync(cedula);
            if (empleado == null)
            {
                TempData["Error"] = $"Empleado con cédula {cedula} no encontrado.";
                return RedirectToAction(nameof(Index));
            }
            return View(empleado); 
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string cedula)
        {
            try
            {
                await _empleadoService.DeleteEmpleadoAsync(cedula);
                TempData["Success"] = "Empleado eliminado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                TempData["Error"] = $"Error al eliminar: Empleado con cédula {cedula} no encontrado.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}