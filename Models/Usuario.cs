using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Web_de_Nominas.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        //Sirve cosa vieja
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Contra { get; set; } = string.Empty;
        public bool CorreoConfirmacion { get; set; }
        public string? CorreoConfirmacionToken { get; set; } = string.Empty;
        public string? RecargaToken { get; set; } = string.Empty;
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? RecargaTokenExpirado { get; set; }
        public string? ContraRecargaToken { get; set; } = string.Empty;
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ReincioTokenExpirado { get; set; }
        public int RolId { get; set; }
        public virtual Rol Rol { get; set; }

        public Collection<Empleado> Empleados { get; set; }

        // Tengo hambre (￣o￣) . z Z
    }
}
