using System.Diagnostics;
using ReservaLaboratorioWilbertMartin.Models;
using Microsoft.AspNetCore.Mvc;

namespace ReservaLaboratorioWilbertMartin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult UserEntrada() { return View(); }

        public IActionResult ConsultasDoc() { return View(); }

        public IActionResult ReservarLab() { return View();  }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
