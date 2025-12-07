using Microsoft.EntityFrameworkCore;
using GestorTareas.Models;

namespace GestorTareas.Data
{
	public class AppDbContext : DbContext
	{
	
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		
		public AppDbContext() { }

	
		public DbSet<Tarea> Tareas { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlite("Data Source=gestor_tareas.db");
			}
		}
	}
}