using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using GestorTareas.Controllers;
using GestorTareas.Data;
using GestorTareas.Models;
using System.Threading.Tasks;
using System.Linq;

namespace GestorTareas.Tests
{
    public class PruebasTareas
    {
        private AppDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new AppDbContext(options);
            databaseContext.Database.EnsureCreated();

            return databaseContext;
        }

        [Fact]
        public void CrearTarea_GuardaEnBaseDeDatos()
        {
            // 1. Arrange
            var context = GetDatabaseContext();
            var controller = new HomeController(context);
            var nuevaTarea = new Tarea
            {
                Titulo = "Tarea Test",
                Descripcion = "Probando",
                NivelPrioridad = Prioridad.Alta,
                EstaCompletada = false
            };

            // 2. Act
            var resultado = controller.Crear(nuevaTarea);

            // 3. Assert
            var conteo = context.Tareas.Count();
            Assert.Equal(1, conteo);
        }

        [Fact]
        public void EliminarTarea_BorraDeBaseDeDatos()
        {
            // 1. Arrange
            var context = GetDatabaseContext();
            var controller = new HomeController(context);
            var tarea = new Tarea { Titulo = "Borrar", NivelPrioridad = Prioridad.Baja };
            context.Tareas.Add(tarea);
            context.SaveChanges();

            // 2. Act
            controller.Eliminar(tarea.Id);

            // 3. Assert
            Assert.Equal(0, context.Tareas.Count());
        }
    }
}