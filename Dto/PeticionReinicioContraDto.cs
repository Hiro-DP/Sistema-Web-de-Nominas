using System.ComponentModel.DataAnnotations;

namespace Sistema_Web_de_Nominas.Dto
{
    public class PeticionReinicioContraDto
    {
        public string Correo { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string NuevaContra { get; set; } = null!;
    }
}
