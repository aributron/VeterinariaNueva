using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace VeterinariaNueva.Models
{
    [Table("Clientes")]
    public class Cliente : Usuario
    {
        public override Rol Rol => Rol.Cliente;
        

    }


}
