using Sistema_Web_de_Nominas.Models;

namespace Sistema_Web_de_Nominas.Services
{
    public interface ITokenService
    {
        string GenerarTokenAcceso(Usuario usuario);
        string GenerarTokenActualizacion();
        string GenerarTokenReinicioContra();
    }
}
