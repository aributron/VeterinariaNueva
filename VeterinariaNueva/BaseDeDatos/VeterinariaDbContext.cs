using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using VeterinariaNueva.Models;

namespace VeterinariaNueva.BaseDatos {
    public class VeterinariaDbContext : DbContext
    {
        public VeterinariaDbContext(DbContextOptions opciones) : base(opciones)
        {

        }

        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }

        public DbSet<Administrador> Administradores { get; set; }
    }
}
