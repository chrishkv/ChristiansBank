using System.ComponentModel.DataAnnotations;

namespace Cliente.Models.Data
{
    public class ClienteModel
    {
        [Key]
        public int clienteid { get; set; }
        public string contrasena { get; set;}
        public string estado { get; set;}
        public PersonaModel persona { get; set;}
    }
}