namespace Sistema_Web_de_Nominas.Dto
{
    public class EmpleadoResponseDTO
    {
        public string Cedula { get; set; } = string.Empty;

        public string NombreCompleto { get; set; } = string.Empty;

        public int Telefono { get; set; }

        public string Sexo { get; set; } = string.Empty;

        public string Cargo { get; set; } = string.Empty;
    }
}
