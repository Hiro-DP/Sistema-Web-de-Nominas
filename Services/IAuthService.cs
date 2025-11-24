using Sistema_Web_de_Nominas.Dto;

namespace Sistema_Web_de_Nominas.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string? ErrorMessage)> RegisterAsync(PeticionRegistroDto dto);
        Task<(bool Success, RespuestaLoginDto? Result, string? ErrorMessage)> LoginAsync(PeticionLoginDto dto);
        Task<bool> ConfirmEmailAsync(string correo, string token);
        Task<bool> SendResetPasswordLinkAsync(string correo);
        Task<bool> ResetPasswordAsync(PeticionReinicioContraDto dto);
        Task<bool> ResendConfirmationEmailAsync(string correo);
        Task<bool> ChangePasswordAsync(string nombreusuario, CambiarContraDto dto);
        Task<RespuestaLoginDto?> RefreshAccessTokenAsync(string refreshToken);
    }
}
