using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppSistemaReservasRestaurente.Data;
using AppSistemaReservasRestaurente.Models;
using Microsoft.AspNetCore.Authorization;

namespace AppSistemaReservasRestaurente.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class HorariosController : Controller
    {
        private readonly BDContexto _context;

        public HorariosController(BDContexto context)
        {
            _context = context;
        }

        // GET: Horarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Horarios.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetReservas(DateTime fecha, string zona)
        {
            // Validar zona → rango de mesas
            var rangos = new Dictionary<string, (int min, int max)>
            {
                ["salon-principal"] = (1, 8),
                ["terraza"] = (9, 16),
                ["balcon"] = (17, 20)
            };

            if (!rangos.TryGetValue(zona ?? "", out var r))
                return Json(Array.Empty<object>());

            var datos = await _context.Reservas
                .Include(x => x.Mesa)
                .Include(x => x.Horario)
                .Include(x => x.Cliente)
                .Where(x =>
                    x.Fecha.Date == fecha.Date &&
                    x.Mesa.ID_Mesa >= r.min && x.Mesa.ID_Mesa <= r.max)
                .Select(x => new
                {
                    // Hora en formato "HH:mm"
                    hora = x.Horario.Hora_Inicio.ToString("HH:mm"),
                    mesa = x.Mesa.ID_Mesa,
                    nombreCliente = x.Cliente.Nombre_Cliente,
                    cantidadPersonas = x.Cantidad_Personas,
                    telefono = x.Cliente.Telefono,
                    email = "", // si luego agregas email en Cliente, lo colocas aquí
                    codigo = x.ID_Reserva,
                    estado = x.Estado // Confirmado | Pendiente | Cancelada
                })
                .ToListAsync();

            return Json(datos);
        }

        // GET: Horarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios
                .FirstOrDefaultAsync(m => m.ID_Horario == id);
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
        public async Task<IActionResult> Create([Bind("ID_Horario,Hora_Inicio,Hora_Final")] Horario horario)
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
        public async Task<IActionResult> Edit(int id, [Bind("ID_Horario,Hora_Inicio,Hora_Final")] Horario horario)
        {
            if (id != horario.ID_Horario)
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
                    if (!HorarioExists(horario.ID_Horario))
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
                .FirstOrDefaultAsync(m => m.ID_Horario == id);
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
            return _context.Horarios.Any(e => e.ID_Horario == id);
        }
       

    }
}
