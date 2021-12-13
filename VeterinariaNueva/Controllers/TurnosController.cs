using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VeterinariaNueva.BaseDatos;
using VeterinariaNueva.Helper;
using VeterinariaNueva.Models;

namespace VeterinariaNueva.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TurnosController : Controller
    {
        private readonly VeterinariaDbContext _context;

        public TurnosController(VeterinariaDbContext context)
        {
            _context = context;
        }

        // GET: Turnos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Turnos.ToListAsync());
        }

        // GET: Turnos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .FirstOrDefaultAsync(m => m.Id_Turno == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // GET: Turnos/Create
        [Authorize]
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Turnos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> Create(Turno turno, int tipo)
        {
            Cliente cliente = RNUsuarios.ObtenerCliente(_context, SessionHelper.GetName(User));
           
            TimeSpan ts = new TimeSpan(turno.Fecha_Hora.Hour, 0, 0);
            turno.Fecha_Hora = turno.Fecha_Hora.Date + ts;          
            turno.Id_Cliente = cliente.Id;
            if (this.ValidarHorario(turno.Fecha_Hora.Hour))
            {
                RNMascotas.AgregarMascota(_context, cliente, turno.Nombre_Mascota, tipo);
                //Si ya existe la mascota para ese cliente, simplemente crea el turno y no la mascota

                if (ModelState.IsValid)
                {
                    if (this.ValidarFecha(turno))
                    {
                    _context.Add(turno);
                    await _context.SaveChangesAsync();               
                    return RedirectToAction(nameof(Index), "Home");
                    }
                    else
                    {
                        ViewBag.Error = "¡Esa hora ya está reservada!";
                        return View(turno);
                    }
                }

            }
            else
            {
                ViewBag.Error = string.Format("¡Horario inválido! Nuestro horario es de {0} a {1}.", Turno.HORARIO_APERTURA, Turno.HORARIO_CIERRE);
                return View(turno);
            }             
                
            return View(turno);
        }

        // GET: Turnos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
            {
                return NotFound();
            }
            return View(turno);
        }

        // POST: Turnos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_Turno,Fecha_Hora,Id_Cliente,Nombre_Mascota")] Turno turno)
        {
            if (id != turno.Id_Turno)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurnoExists(turno.Id_Turno))
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
            return View(turno);
        }

        // GET: Turnos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .FirstOrDefaultAsync(m => m.Id_Turno == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // POST: Turnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurnoExists(int id)
        {
            return _context.Turnos.Any(e => e.Id_Turno == id);
        }

        private bool ValidarFecha(Turno turno)
        {
            List<Turno> listaTurnos = _context.Turnos.ToList();

            var noExistia = !listaTurnos.Any(t => t.Fecha_Hora == turno.Fecha_Hora);

            return noExistia;
        }

        private bool ValidarHorario(int hora)
        {
            bool esValido = false;

            if (hora >= Turno.HORARIO_APERTURA && hora <= Turno.HORARIO_CIERRE)
                esValido = true;

            return esValido;

        }

        public ActionResult HistorialClientes(int id)
        {  
            var lista = new ViewModel.VMHistorialList();                    
            List<Turno> turnos = _context.Turnos.Where(o => o.Id_Cliente == id).ToList();
            lista.Historial = new List<ViewModel.VMHistorial>();
            var cliente = RNUsuarios.ObtenerCliente(_context, turnos.ElementAt(0).Id_Cliente);
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
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HistorialClientes(ViewModel.VMHistorialList lista)
        {

            return RedirectToAction(nameof(HistorialClientes));
        }

        public ActionResult HistorialMascotas(int id, string nombreMascota)
        {
            var lista = new ViewModel.VMHistorialList();
            List<Turno> turnos = _context.Turnos.Where(o => o.Id_Cliente == id && nombreMascota == o.Nombre_Mascota).ToList();
            lista.Historial = new List<ViewModel.VMHistorial>();
            var mascota = RNMascotas.ObtenerMascota(_context, turnos.ElementAt(0).Id_Cliente, nombreMascota);
            var cliente = RNUsuarios.ObtenerCliente(_context, turnos.ElementAt(0).Id_Cliente);
            ViewBag.Mascota = mascota.Nombre;

            foreach (var turno in turnos)
            {
                ViewModel.VMHistorial modelo = new ViewModel.VMHistorial();

                modelo.Cliente = cliente;
                modelo.Mascota = mascota;
                modelo.Turno = turno.Fecha_Hora;

                lista.Historial.Add(modelo);
            }

            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HistorialMascotas(ViewModel.VMHistorialList lista)
        {

            return RedirectToAction(nameof(HistorialMascotas));
        }



    }

}

