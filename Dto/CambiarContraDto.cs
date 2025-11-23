using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Web_de_Nominas.Dto
{
    public class CambiarContraDto
    {
        [Required(ErrorMessage = "Contraseña actual")]
        public string ContraActual { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contraseña Nueva")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener minimo 8 caracteres")]
        public string ContraNueva{ get; set; } = string.Empty;
    }
}
