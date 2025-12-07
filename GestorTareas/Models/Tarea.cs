using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace GestorTareas.Models
{
    public enum Prioridad
    {
        Baja = 0,   
        Media = 1,
        Alta = 2
    }

    public class Tarea
    {
        // EF Core detecta automáticamente que "Id" es la Llave Primaria (PK)
        [Key] 
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [MaxLength(100)] 
        public string Titulo { get; set; } = string.Empty; 

        [MaxLength(500)] 
        public string? Descripcion { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Vencimiento")]
        public DateTime? FechaVencimiento { get; set; }

        // EF Core guardará esto como un NÚMERO (0, 1, 2) en la base de datos
        public Prioridad NivelPrioridad { get; set; }

        public bool EstaCompletada { get; set; }

   
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}