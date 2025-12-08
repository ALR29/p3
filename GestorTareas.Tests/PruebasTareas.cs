using Xunit;
using Microsoft.EntityFrameworkCore;
using GestorTareas.Controllers;
using GestorTareas.Data;
using GestorTareas.Models;
using System.Linq;
using System;

namespace GestorTareas.Tests
{
    public class PruebasTareas
    {
        private AppDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new AppDbContext(options);
            databaseContext.Database.EnsureCreated();

            return databaseContext;
        }

        [Fact]
        public void HU01_CrearTarea_DebeGuardarEnBaseDeDatos()
        {
            var context = GetDatabaseContext();
            var controller = new HomeController(context);
            var nuevaTarea = new Tarea
            {
                Titulo = "Comprar leche",
                Descripcion = "Ir al super",
                NivelPrioridad = Prioridad.Media,
                FechaVencimiento = DateTime.Now.AddDays(1)
            };

            controller.Crear(nuevaTarea);

            var cantidad = context.Tareas.Count();
            Assert.Equal(1, cantidad);
            Assert.Equal("Comprar leche", context.Tareas.First().Titulo);
        }

        // --- AQUÍ ESTABA EL ERROR, YA CORREGIDO ---
        [Fact]
        public void HU02_EditarTarea_DebeActualizarTitulo()
        {
            // 1. Arrange
            var context = GetDatabaseContext();
            var controller = new HomeController(context);

            var tareaOriginal = new Tarea { Titulo = "Titulo Viejo", NivelPrioridad = Prioridad.Baja };
            context.Tareas.Add(tareaOriginal);
            context.SaveChanges();

            // CORRECCIÓN: Limpiamos el rastreador para evitar el error de "Already tracked"
            context.ChangeTracker.Clear();

            var tareaEditada = new Tarea
            {
                Id = tareaOriginal.Id,
                Titulo = "Titulo Nuevo Editado",
                NivelPrioridad = Prioridad.Alta
            };

            // 2. Act
            controller.Editar(tareaEditada);

            // 3. Assert
            var tareaEnDb = context.Tareas.First();
            Assert.Equal("Titulo Nuevo Editado", tareaEnDb.Titulo);
            Assert.Equal(Prioridad.Alta, tareaEnDb.NivelPrioridad);
        }

        [Fact]
        public void HU03_EliminarTarea_DebeBorrarDeBaseDeDatos()
        {
            var context = GetDatabaseContext();
            var controller = new HomeController(context);

            var tarea = new Tarea { Titulo = "Tarea para borrar", NivelPrioridad = Prioridad.Baja };
            context.Tareas.Add(tarea);
            context.SaveChanges();

            // Limpiamos también aquí por seguridad
            context.ChangeTracker.Clear();

            controller.Eliminar(tarea.Id);

            Assert.Equal(0, context.Tareas.Count());
        }

        [Fact]
        public void HU04_MarcarCompletada_DebeCambiarEstadoATrue()
        {
            var context = GetDatabaseContext();
            var controller = new HomeController(context);

            var tarea = new Tarea { Titulo = "Tarea Pendiente", EstaCompletada = false };
            context.Tareas.Add(tarea);
            context.SaveChanges();

            // Limpiamos también aquí por seguridad
            context.ChangeTracker.Clear();

            controller.Completar(tarea.Id);

            var tareaActualizada = context.Tareas.First();
            Assert.True(tareaActualizada.EstaCompletada);
        }
    }
}