using Sistema_Web_de_Nominas.Dto;
using Sistema_Web_de_Nominas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sistema_Web_de_Nominas.Controllers
{
    public class AuthController(IAuthService authService) : Controller
    {
        private readonly IAuthService _authService = authService;

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] PeticionRegistroDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, error) = await _authService.RegisterAsync(dto);
            if (!success)
            {
                return Conflict(new { Message = error });
            }

            return Ok(new { success = true });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            var token = Request.Cookies["access_token"];
            if (!string.IsNullOrEmpty(token)) { return RedirectToAction("Index", "Home"); }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] PeticionLoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errorMessage = "Datos invalidos" });
            }
            var (success, result, error) = await _authService.LoginAsync(dto);
            if (!success)
            {
                return Unauthorized(new { success = false, errorMessage = error ?? "Credenciales incorrectas" });
            }
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = result.ExpiraEn
            };

            Response.Cookies.Append("access_token", result.TokenAcceso, cookieOptions); //cuidado
            Response.Cookies.Append("refresh_token", result.TokenActualizacion, cookieOptions);

            return Ok(new { success = true, message = "Registro Exitoso" });
        }
        [Authorize]
        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            return RedirectToAction("Index", "Home");
        }
        //CONFIGURACION CORREO
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Parametros Invalidos");
            }
            var confirmado = await _authService.ConfirmEmailAsync(email, token);
            if (confirmado)
            {
                TempData["Success"] = "¡Correo confirmado con exito! Ahora puedes iniciar sesion";
                return RedirectToAction("Login");
            }
            TempData["Error"] = "El token es invalido o ya expiro";
            return RedirectToAction("Login");
        }

        //RECUPERAR CONTRASEÑA
        [HttpGet]
        public IActionResult ForgotPassword() => View();
        //Primer paso para hacer la recuperacion por olvidar password
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] PeticionContraOlvidadaDto model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Correo))
            {
                return BadRequest(new { success = false, errorMessage = "El correo es requerido." });
            }

            var sent = await _authService.SendResetPasswordLinkAsync(model.Correo);

            if (sent)
            {
                return Ok(new { success = true });
            }
            else
            {
                return NotFound(new { success = false, errorMessage = "Correo no encontrado." });
            }
        }
        //REINICIAR CONTRASEÑA
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string token)
        {
            var model = new PeticionReinicioContraDto { Correo = email, Token = token };
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] PeticionReinicioContraDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var success = await _authService.ResetPasswordAsync(dto);
            if (success)
            {
                return Ok(new { success = true });
            }
            ModelState.AddModelError("", "Token invalido o expirado");
            return View(dto);
        }
        //CAMBIO CONTRASEÑA
        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword() => View();
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(CambiarContraDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var username = User.Identity?.Name;
            var success = await _authService.ChangePasswordAsync(username, dto);
            if (success)
            {
                TempData["Success"] = "Contraseña actualizada correctamente";
                return RedirectToAction("Logout");
            }

            ModelState.AddModelError("", "Contraseña actual incorrecta");
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized();
            }

            var newTokens = await _authService.RefreshAccessTokenAsync(refreshToken);
            if (newTokens == null)
            {
                return Unauthorized();
            }

            var accessOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = newTokens.ExpiraEn
            };

            var refreshOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = newTokens.ExpiraEn
            };

            Response.Cookies.Append("access_token", newTokens.TokenAcceso, accessOptions);
            Response.Cookies.Append("refresh_token", newTokens.TokenActualizacion, refreshOptions);

            return Ok();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}
