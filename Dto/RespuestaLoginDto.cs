namespace Sistema_Web_de_Nominas.Dto
{
    public class RespuestaLoginDto
    {
        public string TokenAcceso { get; set; } = string.Empty;
        public string TokenActualizacion { get; set; } = string.Empty;
        public DateTime ExpiraEn { get; set; }

    }
}
