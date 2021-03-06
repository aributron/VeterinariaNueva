using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VeterinariaNueva.BaseDatos;
using VeterinariaNueva.Models;
using VeterinariaNueva.Helper;
using Microsoft.AspNetCore.Authorization;

namespace VeterinariaNueva.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ClientesController : Controller
    {
        private readonly VeterinariaDbContext _context;

        public ClientesController(VeterinariaDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

       
        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {

                if (!this.ValidarCliente(cliente))
                {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                } else
                {
                    ViewBag.Error = "Error: Cliente existente";
                }

            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            var mascotas = _context.Mascotas.Where(e => e.Id_Cliente == id).ToList();
            if (mascotas.Count > 0)
            {
                ViewBag.Mascota = "Borrar mascotas";
                return View(cliente);
            }


            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }

        private bool ValidarCliente(Cliente cliente)
        {
            bool existe = false;
            var clientes = _context.Clientes.ToList();

            existe = clientes.Any(e => e.Email == cliente.Email);

            return existe;
        }

        [Authorize]
        [AllowAnonymous]
        public ActionResult HistorialCliente ()
        {
            Cliente cliente = RNUsuarios.ObtenerCliente(_context, SessionHelper.GetName(User));
            if (cliente == null)
            {
                return RedirectToAction(nameof(LoginController.Login), "Login");
            }
            int id = cliente.Id;
            var lista = new ViewModel.VMHistorialList();
            List<Turno> turnos = _context.Turnos.Where(o => o.Id_Cliente == id).ToList();
            lista.Historial = new List<ViewModel.VMHistorial>();
            
            ViewBag.Cliente = cliente.Email;

            foreach (var turno in turnos)
            {
                ViewModel.VMHistorial modelo = new ViewModel.VMHistorial();

                var mascota = RNMascotas.ObtenerMascota(_context, turno.Nombre_Mascota);

                modelo.Cliente = cliente;
                modelo.Mascota = mascota;
                modelo.Turno = turno.Fecha_Hora;

                lista.Historial.Add(modelo);
            }

            return View(lista);

        }
        [Authorize]
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HistorialCliente(ViewModel.VMHistorialList lista)
        {

            return RedirectToAction(nameof(HistorialCliente));
        }

        public async Task<IActionResult> DeleteMascota(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas
                .FirstOrDefaultAsync(m => m.Id_Mascota == id);
            if (mascota == null)
            {
                return NotFound();
            }

            return View(mascota);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("DeleteMascota")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmado(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);
            _context.Mascotas.Remove(mascota);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Turnos");
        }



    }
}
