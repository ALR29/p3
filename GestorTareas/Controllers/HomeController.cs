using GestorTareas.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GestorTareas.Controllers
{
    public class HomeController : Controller
    {
        // Simulamos la base de datos con una lista estática
        private static List<Tarea> tareas = new List<Tarea>();

        public IActionResult Index()
        {
            return View(tareas); // Pasamos la lista a la vista
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}