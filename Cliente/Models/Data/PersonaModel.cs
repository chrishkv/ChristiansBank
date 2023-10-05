using System.ComponentModel.DataAnnotations;

namespace Cliente.Models.Data
{
    public class PersonaModel
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        [MaxLength(10)]
        public string? Genero { get; set; }
        public int? Edad { get; set; }
        public string Identificacion { get; set; }
        public string Direccion { get; set; }
        [MaxLength(15)]
        public string Telefono { get; set; }
    }
}
