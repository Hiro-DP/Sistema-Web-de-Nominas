using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Web_de_Nominas.Models
{
    [Table("Empleado")]
    public class Empleado
    {
        
        [Key]
        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(20)]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;

       
        public int Telefono { get; set; }

        [Required(ErrorMessage = "El sexo es obligatorio.")]
        [StringLength(10)]
        public string Sexo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El cargo es obligatorio.")]
        [StringLength(50)]
        public string Cargo { get; set; } = string.Empty;

    }
}
