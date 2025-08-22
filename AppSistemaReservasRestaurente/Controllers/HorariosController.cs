using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppSistemaReservasRestaurente.Data;
using AppSistemaReservasRestaurente.Models;

namespace AppSistemaReservasRestaurente.Controllers
{
    public class HorariosController : Controller
    {
        private readonly BDContexto _context;

        public HorariosController(BDContexto context)
        {
            _context = context;
        }

        // GET: Horarios
        public IActionResult Index(DateTime? fecha, string zona)
        {
            var fechaSeleccionada = fecha ?? DateTime.Today;

            var reservas = _context.Reservas
               .Include(r => r.Cliente)
               .Include(r => r.Mesa)
               .Include(r => r.Horario)
               .Where(r => r.Fecha.Date == fechaSeleccionada.Date);

            if (!string.IsNullOrEmpty(zona))
            {
                reservas = reservas.Where(r => r.Mesa!.Zona == zona);
            }

            return View(reservas.ToList());
        }

        // GET: Detalles de una reserva
        public IActionResult Detalles(int id)
        {
            var reserva = _context.Reservas
                 .Include(r => r.Cliente)
                 .Include(r => r.Mesa)
                 .Include(r => r.Horario)
                 .FirstOrDefault(r => r.ReservaId == id);

            if (reserva == null)
            {
                return NotFound();
            }

            return PartialView("_DetallesReserva", reserva);
        }
        [HttpPost]
        public IActionResult CambiarEstado(int id, string nuevoEstado)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva == null) return NotFound();

            reserva.Estado = nuevoEstado;
            _context.SaveChanges();

            return Ok(new { success = true, estado = nuevoEstado });
        }

        // Borrar reserva (dejar mesa libre)
        [HttpPost]
        public IActionResult Borrar(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva == null) return NotFound();

            _context.Reservas.Remove(reserva);
            _context.SaveChanges();

            return Ok(new { success = true });
        }
    }
}
        
        /*private readonly BDContexto _context;

        public HorariosController(BDContexto context)
        {
            _context = context;
        }

        // GET: Horarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Horarios.ToListAsync());
        }

        // GET: Horarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios
                .FirstOrDefaultAsync(m => m.HorarioId == id);
            if (horario == null)
            {
                return NotFound();
            }

            return View(horario);
        }

        // GET: Horarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Horarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HorarioId,HoraInicio,HoraFin")] Horario horario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(horario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(horario);
        }

        // GET: Horarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null)
            {
                return NotFound();
            }
            return View(horario);
        }

        // POST: Horarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HorarioId,HoraInicio,HoraFin")] Horario horario)
        {
            if (id != horario.HorarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(horario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorarioExists(horario.HorarioId))
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
            return View(horario);
        }

        // GET: Horarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios
                .FirstOrDefaultAsync(m => m.HorarioId == id);
            if (horario == null)
            {
                return NotFound();
            }

            return View(horario);
        }

        // POST: Horarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);
            if (horario != null)
            {
                _context.Horarios.Remove(horario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorarioExists(int id)
        {
            return _context.Horarios.Any(e => e.HorarioId == id);
        }
        
    }
}*/
