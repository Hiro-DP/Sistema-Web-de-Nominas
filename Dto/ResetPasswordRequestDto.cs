using System.ComponentModel.DataAnnotations;

namespace AppAutenticaciones.Dto
{
    public class ResetPasswordRequestDto
    {
        /*
         public string Email
         public string Token
         public string NewPassword
         */

        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

        [MinLength(8, ErrorMessage = "La contraseña debe tener minimo 8 caracteres")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
