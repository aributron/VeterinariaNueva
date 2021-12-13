using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinariaNueva.Models;

namespace VeterinariaNueva.ViewModel
{
    public class VMHistorial
    {
        public DateTime Turno { get; set; }
        public Cliente Cliente { get; set; }
        public Mascota Mascota { get; set; }
    }

    public class VMHistorialList
    {
        public List<VMHistorial> Historial { get; set; }
    }
}
