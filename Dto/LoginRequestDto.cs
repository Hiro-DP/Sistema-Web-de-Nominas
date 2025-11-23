using System.ComponentModel.DataAnnotations;

namespace AppAutenticaciones.Dto
{
    public class LoginRequestDto
    {
        /*
         public string Email
         public string Password
         */

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
