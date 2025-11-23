using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Web_de_Nominas.Models
{
    [Table("Rol")]
    public class Rol
    {
        //Prueba 2

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public ICollection<Usuario> Usuario { get; set; }

    }
}
