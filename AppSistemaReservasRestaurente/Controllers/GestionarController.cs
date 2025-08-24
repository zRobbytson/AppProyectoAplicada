using Microsoft.AspNetCore.Mvc;

namespace AppSistemaReservasRestaurente.Controllers
{
    public class GestionarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
