using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VeterinariaNueva.Models
{
    [Table("Mascotas")]
    public class Mascota
    {
        [Key]
        public int Id_Mascota { get; set; }
        [ForeignKey(nameof(Cliente))]
        public int Id_Cliente { get; set; }
        public string Nombre { get; set; }
        public EnumMascota Tipo_Mascota { get; set; }

        public enum EnumMascota
        {
            Perro,
            Gato,
            Hamster
        };
    }
}
