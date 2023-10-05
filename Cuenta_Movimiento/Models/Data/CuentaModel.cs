using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Cuenta_Movimiento.Models.Data
{
    [Index(nameof(numero_cuenta), IsUnique = true)]
    public class CuentaModel
    {
        [Key]
        public int cuenta_id { get; set; }
        public int numero_cuenta { get; set; }
        [MaxLength(10)]
        public string tipo_cuenta { get; set; }
        public float saldo_inicial {  get; set; }
        public bool estado { get; set; }
        public int cliente_id { get; set; }
    }
}