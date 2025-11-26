namespace Sistema_Web_de_Nominas.Dto
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public int RolId { get; set; }

    }
}
