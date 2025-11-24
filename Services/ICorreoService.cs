namespace Sistema_Web_de_Nominas.Services
{
    public interface ICorreoService
    {
        void EnvioCorreoReinicioContra(string toEmail, string body);
    }
}
