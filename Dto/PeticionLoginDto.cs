using System.ComponentModel.DataAnnotations;

namespace Sistema_Web_de_Nominas.Dto
{
    public class PeticionLoginDto
    {
        public string Correo { get; set; } = string.Empty;
        public string Contra { get; set; } = string.Empty;
    }
}
