using System.Security.Cryptography;
using Sistema_Web_de_Nominas.Repositorio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sistema_Web_de_Nominas.Dto;
using Sistema_Web_de_Nominas.Models;
using Sistema_Web_de_Nominas.Services;

namespace Sistema_Web_de_Nominas.Services
{
    public class AuthService(IUsuarioRepository usuarioRepo, ITokenService tokenService, ICorreoService correoService, IMapper mapper) : IAuthService
    {

        private readonly IUsuarioRepository _userRepo = usuarioRepo;
        private readonly ITokenService _tokenService = tokenService;
        private readonly ICorreoService _emailService = correoService;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> ChangePasswordAsync(string nombreusuario, CambiarContraDto dto)
        {
            var user = await _userRepo.GetUserByUserName(nombreusuario);
            if (user == null || !_userRepo.ValidatePassword(user, dto.ContraActual))
                return false;

            user.Contra = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.ContraNueva);
            await _userRepo.SaveAsync();
            return true;
        }

        public async Task<bool> ConfirmEmailAsync(string correo, string token)
        {
            var user = await _userRepo.GetUserByEmail(correo);
            if (user == null || user.CorreoConfirmacionToken != token)
                return false;

            var decodedToken = Uri.UnescapeDataString(token.Trim());

            if (!string.Equals(user.CorreoConfirmacionToken, decodedToken, StringComparison.Ordinal))
                return false;

            user.CorreoConfirmacion = true;
            user.CorreoConfirmacionToken = null;
            await _userRepo.SaveAsync();

            return true;
        }

        public async Task<(bool Success, RespuestaLoginDto? Result, string? ErrorMessage)> LoginAsync(PeticionLoginDto dto)
        {
            var user = await _userRepo.GetUserByEmail(dto.Correo);
            if (user == null || !_userRepo.ValidatePassword(user, dto.Contra))
                return (false, null, "Credenciales inválidas");

            if (!user.CorreoConfirmacion)
                return (false, null, "Debe confirmar su correo electrónico.");

            var accessToken = _tokenService.GenerarTokenAcceso(user);
            var refreshToken = _tokenService.GenerarTokenActualizacion();

            user.RecargaToken = refreshToken;
            user.RecargaTokenExpirado = DateTime.Now.AddDays(7);
            await _userRepo.SaveAsync();

            var result = new RespuestaLoginDto
            {
                TokenAcceso = accessToken,
                TokenActualizacion = refreshToken,
                ExpiraEn = DateTime.Now.AddMinutes(15)
            };

            return (true, result, null);
        }

        public async Task<RespuestaLoginDto?> RefreshAccessTokenAsync(string refreshToken)
        {
            var user = await _userRepo.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null || user.RecargaTokenExpirado < DateTime.UtcNow)
                return null;

            var newAccessToken = _tokenService.GenerarTokenAcceso(user);
            var newRefreshToken = _tokenService.GenerarTokenActualizacion();

            user.RecargaToken = newRefreshToken;
            user.RecargaTokenExpirado = DateTime.UtcNow.AddDays(7);

            await _userRepo.SaveAsync();

            return new RespuestaLoginDto
            {
                TokenAcceso = newAccessToken,
                TokenActualizacion = newRefreshToken,
                ExpiraEn = DateTime.UtcNow.AddMinutes(15)
            };

        }

        public async Task<(bool Success, string? ErrorMessage)> RegisterAsync(PeticionRegistroDto dto)
        {
            var exists = await _userRepo.GetUserByEmail(dto.Correo);
            if (exists != null)
                return (false, "Correo ya en uso, Intente con otro por favor");

            var user = _mapper.Map<Usuario>(dto);
            user.Contra = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Contra);

            //Generamos el token a enviar
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var rawToken = Convert.ToBase64String(tokenBytes);
            var encodedToken = Uri.EscapeDataString(rawToken);

            user.CorreoConfirmacionToken = rawToken;
            user.CorreoConfirmacion = false;

            await _userRepo.AddAsync(user);
            await _userRepo.SaveAsync();
            string confirmationLink = $"https://localhost:7130/Auth/ConfirmEmail?email={user.Correo}&token={encodedToken}";

            string htmlBody = $"""
                            <!DOCTYPE html>
                            <html><body>
                            <p>Hola, </p>
                            <p>Haz clic en el siguiente boton para confirmar tu cuenta:</p>
                            <p><a href="{confirmationLink}" style='background:#007BFF;color:white;padding:10px 20px;border-radius:5px;text-decoration:none;'>Confirmar Cuenta<</a></p>
                            </body></html>
                            """;

            _emailService.EnvioCorreoReinicioContra(user.Correo, htmlBody);

            return (true, null);
        }

        public async Task<bool> ResendConfirmationEmailAsync(string correo)
        {
            var user = await _userRepo.GetUserByEmail(correo);
            if (user == null || user.CorreoConfirmacion)
                return false;

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var rawToken = Convert.ToBase64String(tokenBytes);
            var encodedToken = Uri.EscapeDataString(rawToken);
            user.CorreoConfirmacionToken = rawToken;

            await _userRepo.SaveAsync();

            string confirmationLink = $"https://localhost:7130/Auth/ConfirmEmail?email={user.Correo}&token={encodedToken}"; //ver bien del puerto en el local host

            string htmlBody = $"""
                            <!DOCTYPE html>
                            <html><body>
                            <p>Hola, </p>
                            <p>Haz clic en el siguiente boton para confirmar tu cuenta:</p>
                            <p><a href="{confirmationLink}" style='background:#007BFF;color:white;padding:10px 20px;border-radius:5px;text-decoration:none;'>Confirmar Cuenta<</a></p>
                            </body></html>
                            """;

            _emailService.EnvioCorreoReinicioContra(user.Correo, htmlBody);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(PeticionReinicioContraDto dto)
        {
            var user = await _userRepo.GetUserByEmail(dto.Correo);

            if (user == null)
                return false;

            // Decodificar el token del URL encoding si viene codificado
            var providedToken = Uri.UnescapeDataString(dto.Token ?? string.Empty);
            var storedToken = user.ContraRecargaToken ?? string.Empty;

            // Comparar los tokens
            if (!storedToken.Equals(providedToken, StringComparison.Ordinal))
            {
                return false;
            }

            // Verificar expiración
            if (!user.ReincioTokenExpirado.HasValue || user.ReincioTokenExpirado.Value < DateTime.Now)
            {
                return false;
            }

            user.Contra = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.NuevaContra);
            user.ContraRecargaToken = null;
            user.ReincioTokenExpirado = null;

            await _userRepo.SaveAsync();
            return true;
        }

        public async Task<bool> SendResetPasswordLinkAsync(string correo)
        {
            var user = await _userRepo.GetUserByEmail(correo);
            if (user == null) return false;

            var token = _tokenService.GenerarTokenReinicioContra();
            var encodedToken = Uri.EscapeDataString(token);
            user.ContraRecargaToken = token;
            user.ReincioTokenExpirado = DateTime.UtcNow.AddHours(1);
            await _userRepo.SaveAsync();
            string resetLink = $"https://localhost:7130/Auth/ResetPassword?email={user.Correo}&token={encodedToken}";
            string htmlBody = $"""
                            <!DOCTYPE html>
                            <html><body>
                            <p>Hola, </p>
                            <p>Haz clic en el siguiente boton para restablecer tu contraseña:</p>
                            <p><a href="{resetLink}" style='background:#007BFF;color:white;padding:10px 20px;border-radius:5px;text-decoration:none;'>Restablecer Contraseña<</a></p>
                            </body></html>
                            """;
            _emailService.EnvioCorreoReinicioContra(user.Correo, htmlBody);
            return true;
        }
    }
}
