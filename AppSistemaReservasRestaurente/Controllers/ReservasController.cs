using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppSistemaReservasRestaurente.Data;
using AppSistemaReservasRestaurente.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace AppSistemaReservasRestaurente.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly BDContexto _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservasController(BDContexto context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservas
        //public async Task<IActionResult> Index()
        //{
        //    var bDContexto = _context.Reservas.Include(r => r.Cliente).Include(r => r.Horario).Include(r => r.Mesa);
        //    return View(await bDContexto.ToListAsync());
        //}
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Horario)
                .Include(r => r.Mesa)
                .ToListAsync();

            return View(reservas);
        }


        //Acción para consultar mesas libres
        [HttpGet]
        public async Task<IActionResult> GetMesasLibres(DateTime fecha, int idHorario, string zona)
        {
            var mesasOcupadas = await _context.Reservas
                .Where(r => r.Fecha.Date == fecha.Date && r.ID_Horario == idHorario && r.Mesa.Zona == zona)
                .Select(r => r.ID_Mesa)
                .ToListAsync();

            var mesasLibres = await _context.Mesas
            .Where(m => m.Zona == zona)
            .Select(m => new {
                IdMesa = m.ID_Mesa,   // 🔹 nombre uniforme
                Capacidad = m.Capacidad,
                Zona = m.Zona,
                Ocupada = mesasOcupadas.Contains(m.ID_Mesa)
            })
            .ToListAsync();

            return Json(mesasLibres);

        }



        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Horario)
                .Include(r => r.Mesa)
                .FirstOrDefaultAsync(m => m.ID_Reserva == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        //public IActionResult Create()
        //{
        //    ViewData["ID_Cliente"] = new SelectList(_context.Clientes, "ID_Cliente", "ID_Cliente");
        //    ViewData["ID_Horario"] = new SelectList(_context.Horarios, "ID_Horario", "ID_Horario");
        //    ViewData["ID_Mesa"] = new SelectList(_context.Mesas, "ID_Mesa", "ID_Mesa");
        //    return View();
        //}
        public async Task<IActionResult> Create()
        {
            // Obtenemos el usuario actual
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge(); // si no está logueado, redirige a login

            // Obtenemos el cliente asociado
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == user.Id);

            if (cliente == null)
            {
                // Caso raro: el usuario no tiene cliente
                return RedirectToAction("Error", "Home");
            }

            // 🔹 Llenamos el combo de Mesas con descripción detallada
            ViewData["ID_Mesa"] = new SelectList(
                _context.Mesas.Select(m => new
                {
                    m.ID_Mesa,
                    Descripcion = "Mesa " + m.ID_Mesa + " (" + m.Zona + " - " + m.Capacidad + " personas)"
                }),
                "ID_Mesa",
                "Descripcion"
            );

            // 🔹 Llenamos el combo de Horarios en formato 07:00 - 08:00
            ViewData["ID_Horario"] = new SelectList(
                _context.Horarios.Select(h => new
                {
                    h.ID_Horario,
                    Descripcion = h.Hora_Inicio.ToString().Substring(0, 5) + " - " + h.Hora_Final.ToString().Substring(0, 5)
                }),
                "ID_Horario",
                "Descripcion"
            );
            ViewBag.Horarios = await _context.Horarios.ToListAsync();
            ViewBag.Zonas = await _context.Mesas.Select(m => m.Zona).Distinct().ToListAsync();


            // Mandamos los datos del cliente a la vista
            ViewBag.Cliente = cliente;
            ViewBag.Correo = user.Email;

            return View();
        }


        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID_Reserva,ID_Cliente,ID_Mesa,ID_Horario,Fecha,Cantidad_Personas,Estado")] Reserva reserva)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(reserva);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ID_Cliente"] = new SelectList(_context.Clientes, "ID_Cliente", "ID_Cliente", reserva.ID_Cliente);
        //    ViewData["ID_Horario"] = new SelectList(_context.Horarios, "ID_Horario", "ID_Horario", reserva.ID_Horario);
        //    ViewData["ID_Mesa"] = new SelectList(_context.Mesas, "ID_Mesa", "ID_Mesa", reserva.ID_Mesa);
        //    return View(reserva);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID_Reserva,ID_Mesa,ID_Horario,Fecha,Cantidad_Personas")] Reserva reserva)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null) return Challenge();

        //    var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == user.Id);
        //    if (cliente == null) return RedirectToAction("Error", "Home");

        //    // Forzamos que la reserva quede vinculada al cliente logueado
        //    reserva.ID_Cliente = cliente.ID_Cliente;

        //    // 👇 Siempre asignamos un estado por defecto
        //    reserva.Estado = "Confirmado";

        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(reserva);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ID_Mesa"] = new SelectList(
        //        _context.Mesas.Select(m => new
        //        {
        //            m.ID_Mesa,
        //            Descripcion = "Mesa " + m.ID_Mesa + " (" + m.Zona + " - " + m.Capacidad + " personas)"
        //        }),
        //        "ID_Mesa",
        //        "Descripcion"
        //    );

        //    // 🔹 Llenamos el combo de Horarios en formato 07:00 - 08:00
        //    ViewData["ID_Horario"] = new SelectList(
        //        _context.Horarios.Select(h => new
        //        {
        //            h.ID_Horario,
        //            Descripcion = h.Hora_Inicio.ToString().Substring(0, 5) + " - " + h.Hora_Final.ToString().Substring(0, 5)
        //        }),
        //        "ID_Horario",
        //        "Descripcion"
        //    );
        //    ViewBag.Cliente = cliente;
        //    ViewBag.Correo = user.Email;
        //    return View(reserva);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Mesa,ID_Horario,Fecha,Cantidad_Personas")] Reserva reserva)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == user.Id);
            if (cliente == null) return RedirectToAction("Error", "Home");

            // Vincular cliente logueado
            reserva.ID_Cliente = cliente.ID_Cliente;

            // Estado por defecto
            reserva.Estado = "Confirmado";

            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // En caso de error recargamos combos
            ViewBag.Cliente = cliente;
            ViewBag.Correo = user.Email;
            ViewBag.Horarios = await _context.Horarios.ToListAsync();
            ViewBag.Zonas = await _context.Mesas.Select(m => m.Zona).Distinct().ToListAsync();

            var reservas = await _context.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Horario)
            .Include(r => r.Mesa)
            .ToListAsync();

            return View("Index", reservas);

        }


        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            ViewData["ID_Cliente"] = new SelectList(_context.Clientes, "ID_Cliente", "ID_Cliente", reserva.ID_Cliente);
            ViewData["ID_Horario"] = new SelectList(_context.Horarios, "ID_Horario", "ID_Horario", reserva.ID_Horario);
            ViewData["ID_Mesa"] = new SelectList(_context.Mesas, "ID_Mesa", "ID_Mesa", reserva.ID_Mesa);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Reserva,ID_Cliente,ID_Mesa,ID_Horario,Fecha,Cantidad_Personas,Estado")] Reserva reserva)
        {
            if (id != reserva.ID_Reserva)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.ID_Reserva))
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
            ViewData["ID_Cliente"] = new SelectList(_context.Clientes, "ID_Cliente", "ID_Cliente", reserva.ID_Cliente);
            ViewData["ID_Horario"] = new SelectList(_context.Horarios, "ID_Horario", "ID_Horario", reserva.ID_Horario);
            ViewData["ID_Mesa"] = new SelectList(_context.Mesas, "ID_Mesa", "ID_Mesa", reserva.ID_Mesa);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Horario)
                .Include(r => r.Mesa)
                .FirstOrDefaultAsync(m => m.ID_Reserva == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.ID_Reserva == id);
        }
    }
}
