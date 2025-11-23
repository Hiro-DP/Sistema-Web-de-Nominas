using System.ComponentModel.DataAnnotations;

namespace Sistema_Web_de_Nominas.Dto
{
    public class PeticionReinicioContraDto
    {
        public string Correo { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

        [MinLength(8, ErrorMessage = "La contraseña debe tener minimo 8 caracteres")]
        public string NuevaContra { get; set; } = string.Empty;
    }
}
