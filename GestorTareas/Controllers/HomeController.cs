using GestorTareas.Data;
using GestorTareas.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GestorTareas.Controllers
{
    public class HomeController : Controller
    {
        // Esta es la conexión a la base de datos
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Index
        public IActionResult Index(string buscar, Prioridad? filtroPrioridad, bool? mostrarCompletadas)
        {
         
            var query = _context.Tareas.AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
            {
                query = query.Where(t => t.Titulo.Contains(buscar));
            }

            if (filtroPrioridad.HasValue)
            {
                query = query.Where(t => t.NivelPrioridad == filtroPrioridad.Value);
            }

            if (mostrarCompletadas.HasValue && mostrarCompletadas.Value == false)
            {
                query = query.Where(t => !t.EstaCompletada);
            }

            var lista = query
                .OrderBy(t => t.EstaCompletada)
                .ThenByDescending(t => t.FechaVencimiento)
                .ThenByDescending(t => t.NivelPrioridad)
                .ToList();

            ViewBag.Buscar = buscar;
            ViewBag.Prioridad = filtroPrioridad;
            ViewBag.MostrarCompletadas = mostrarCompletadas;

            return View(lista);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Tarea nuevaTarea)
        {
            if (ModelState.IsValid)
            {
               
                nuevaTarea.EstaCompletada = false;
                _context.Tareas.Add(nuevaTarea);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nuevaTarea);
        }

        public IActionResult Editar(int id)
        {
          
            var tarea = _context.Tareas.Find(id);
            if (tarea == null) return NotFound();
            return View(tarea);
        }

        [HttpPost]
        public IActionResult Editar(Tarea tareaEditada)
        {
            if (ModelState.IsValid)
            {
               
                _context.Tareas.Update(tareaEditada);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tareaEditada);
        }

        public IActionResult Completar(int id)
        {
            var tarea = _context.Tareas.Find(id);
            if (tarea != null)
            {
                tarea.EstaCompletada = !tarea.EstaCompletada;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            var tarea = _context.Tareas.Find(id);
            if (tarea != null)
            {
                _context.Tareas.Remove(tarea);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}