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
            return View(tareas);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // --- AQUI ESTÁN LOS MÉTODOS NUEVOS ---
        public IActionResult Completar(int id)
        {
            var tarea = tareas.FirstOrDefault(t => t.Id == id);
            if (tarea != null)
            {
                // Invertimos el valor: si es true pasa a false, y viceversa
                tarea.EstaCompletada = !tarea.EstaCompletada;
            }
            return RedirectToAction("Index");
        }

        // GET: Muestra el formulario
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Recibe los datos del formulario
        [HttpPost]
        public IActionResult Crear(Tarea nuevaTarea)
        {
            if (ModelState.IsValid)
            {
                // Generar un ID simulado (auto-incrementable)
                // Si la lista está vacía, el ID es 1. Si no, tomamos el último ID y sumamos 1.
                int nuevoId = tareas.Count > 0 ? tareas.Max(t => t.Id) + 1 : 1;

                nuevaTarea.Id = nuevoId;
                nuevaTarea.EstaCompletada = false; // Por defecto no está completa

                tareas.Add(nuevaTarea);
                return RedirectToAction("Index");
            }
            return View(nuevaTarea);
        }

        // ACCIÓN: Eliminar una tarea de la lista
        public IActionResult Eliminar(int id)
        {
            var tarea = tareas.FirstOrDefault(t => t.Id == id);
            if (tarea != null)
            {
                tareas.Remove(tarea);
            }
            return RedirectToAction("Index");
        }
    }
}