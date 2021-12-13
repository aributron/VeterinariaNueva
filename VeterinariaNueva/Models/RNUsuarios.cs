using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinariaNueva.BaseDatos;

namespace VeterinariaNueva.Models
{
    public class RNUsuarios
    {
        public static Usuario ObtenerUsuario(VeterinariaDbContext _context, string email, string password)
        {

            Usuario usuario = _context.Clientes.FirstOrDefault(o => o.Email.ToUpper() == email.ToUpper() && password == o.Password);
            if (usuario == null)
                usuario = _context.Administradores.FirstOrDefault(o => o.Email.ToUpper() == email.ToUpper() && password == o.Password);
            return usuario;
        }

        public static Cliente ObtenerCliente(VeterinariaDbContext _context, string email)
        {

            Cliente cliente = _context.Clientes.FirstOrDefault(o => o.Email.ToUpper() == email.ToUpper());
           
            return cliente;
        }

        public static Cliente ObtenerCliente(VeterinariaDbContext _context, int id)
        {

            Cliente cliente = _context.Clientes.FirstOrDefault(o => o.Id== id);

            return cliente;
        }

    }
}
