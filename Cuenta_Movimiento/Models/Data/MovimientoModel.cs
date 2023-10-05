using System.ComponentModel.DataAnnotations;

namespace Cuenta_Movimiento.Models.Data
{
    public class MovimientoModel
    {
        [Key]
        public int movimiento_id { get; set; }
        public DateTime fecha { get; set; }
        [MaxLength(10)]
        public string tipo_movimiento {  get; set; }
        public float valor {  get; set; }
        public float saldo { get; set; }
        public string descripcion { get; set; }
        public CuentaModel? cuenta { get; set; }
    }
}