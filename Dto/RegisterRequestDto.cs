using System.ComponentModel.DataAnnotations;

namespace AppAutenticaciones.Dto
{
    public class RegisterRequestDto
    {
        /*
        public required string Username
        public required string Email
        public required string Password
        public int RoleId =2
         */

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "El correo electronico es obligatorio")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; } = 2;
    }
}
