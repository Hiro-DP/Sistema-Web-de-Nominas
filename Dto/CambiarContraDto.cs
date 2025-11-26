using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Web_de_Nominas.Dto
{
    public class CambiarContraDto
    {
        public required string ContraActual { get; set; }
        public required string ContraNueva{ get; set; }
    }
}
