namespace Sistema_Web_de_Nominas.Dto
{
    public class NominaResponseDTO
    {
        public int CodigoId { get; set; }

        public decimal Salario { get; set; }

        public double HorasExtras { get; set; }

        public decimal MontoDeHorasExtras { get; set; }

        public decimal Devengado { get; set; }

        public decimal INSSLaboral { get; set; }

        public int Inasistencia { get; set; }

        public decimal MontoDeducciones { get; set; }

        public decimal SalarioNeto { get; set; }

        public string EmpleadoCedula { get; set; } = string.Empty;
    }
}
