using System.ComponentModel.DataAnnotations;

namespace Sistema_Web_de_Nominas.Dto
{
    public class PeticionRegistroDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string Usuario { get; set; } = string.Empty;
        [Required(ErrorMessage = "El correo electronico es obligatorio")]
        public string Correo { get; set; } = string.Empty;
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contra { get; set; } = string.Empty;
        public int RolId { get; set; } = 2;
    }
}
