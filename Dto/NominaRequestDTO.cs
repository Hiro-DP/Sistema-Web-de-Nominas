using System.ComponentModel.DataAnnotations;

namespace Sistema_Web_de_Nominas.Dto
{
    public class NominaRequestDTO
    {
        [Required(ErrorMessage = "El salario base es obligatorio.")]
        public decimal Salario { get; set; }

        public double HorasExtras { get; set; }

        public int Inasistencia { get; set; }

        // La cédula del empleado al que se aplica la nómina
        [Required(ErrorMessage = "La cédula del empleado es obligatoria.")]
        [StringLength(20)]
        public string EmpleadoCedula { get; set; } = string.Empty;

        //exclui estos que son montos calculados MontoDeHorasExtras, Devengado, INSSLaboral, MontoDeducciones, SalarioNeto
    }
}
