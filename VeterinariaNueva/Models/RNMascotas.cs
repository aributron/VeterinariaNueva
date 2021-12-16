using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinariaNueva.BaseDatos;
using static VeterinariaNueva.Models.Mascota;
using VeterinariaNueva.Models;

namespace VeterinariaNueva.Models
{
    public class RNMascotas
    {
        public static void AgregarMascota(VeterinariaDbContext _context, Cliente cliente, string nombre, int tipo)
        {
            if (ValidarMascota(_context, cliente, nombre))
            {
                EnumMascota tipoMascota = (EnumMascota)tipo;

                Mascota mascota = new Mascota()
                {
                    Nombre = nombre,
                    Tipo_Mascota = tipoMascota,
                    Id_Cliente = cliente.Id
                };

                _context.Mascotas.Add(mascota);

            } // else, ya existe esa mascota para ese cliente
        }

        public static void BorrarMascota(VeterinariaDbContext _context, Mascota mascota)
        {
            _context.Mascotas.Remove(mascota);
        }

        private static bool ValidarMascota (VeterinariaDbContext _context, Cliente cliente, string nombre)
        {
            bool agregar = false;
            Mascota mascota = ObtenerMascota(_context, cliente.Id, nombre);

            if (mascota == null)
                agregar = true;
            
            return agregar;
        }

        public static Mascota ObtenerMascota (VeterinariaDbContext _context, string nombre)
        {         
            List<Mascota> mascotas = _context.Mascotas.ToList();
            var mascota = mascotas.FirstOrDefault(e => e.Nombre == nombre);

            return mascota;
        }

        public static Mascota ObtenerMascota(VeterinariaDbContext _context, int idCliente, string nombre)
        {
            List<Mascota> mascotas = _context.Mascotas.Where(o => o.Nombre == nombre).ToList();
            var mascota = mascotas.FirstOrDefault(e => e.Id_Cliente == idCliente);

            return mascota;
        }



    }
}

                
                
                   
                

            
               
            
