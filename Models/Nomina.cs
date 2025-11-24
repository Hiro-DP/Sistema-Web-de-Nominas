using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Web_de_Nominas.Models
{
    [Table("Nomina")]
    public class Nomina
    {
        [Key]
        public int CodigoId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Salario Base")]
        public decimal Salario { get; set; }

        public double HorasExtras { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal MontoDeHorasExtras { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Devengado { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal INSSLaboral { get; set; }

        public int Inasistencia { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal MontoDeducciones { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal SalarioNeto { get; set; }

        [Required]
        [StringLength(20)]
        public string EmpleadoCedula { get; set; } = string.Empty;

        [ForeignKey("EmpleadoCedula")]
        public virtual Empleado Empleado { get; set; }
    }
}
