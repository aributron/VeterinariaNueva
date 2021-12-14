using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VeterinariaNueva.Models
{
    [Table ("Turnos")]
    public class Turno
    {
        public const int HORARIO_APERTURA = 10;
        public const int HORARIO_CIERRE = 19;
        [Key]
        [Display(Name = "Id del turno")]
        public int Id_Turno { get; set; }
        [Display(Name = "Fecha y hora")]
        public DateTime Fecha_Hora { get; set; }

        [ForeignKey(nameof(Cliente))]
        [Display(Name = "Id del cliente")]
        public int Id_Cliente { get; set; }
        [Display(Name = "Nombre de su mascota")]
        public string Nombre_Mascota { get; set; }
        

    }
}
