using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppSistemaReservasRestaurente.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class HistorialesController : Controller
    {
        public IActionResult Index() => View();
        //public async Task<IActionResult> Index()
        //{
        //    var historial = await _context.Reservas
        //        .Include(r => r.Cliente)
        //        .Include(r => r.Mesa)
        //        .Include(r => r.Horario)
        //        .OrderByDescending(r => r.Fecha)
        //        .ToListAsync();

        //    return View(historial);
        //}

    }
}
