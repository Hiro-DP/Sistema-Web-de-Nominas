using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAutenticaciones.Dto
{
    public class ChangePasswordDto
    {
        /*
         public required string CurrentPassword
         public required string NewPassword
         */

        [Required(ErrorMessage = "Contraseña actual")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contraseña Nueva")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener minimo 8 caracteres")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
