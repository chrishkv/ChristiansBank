using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Cuenta_Movimiento.Models.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public DbSet<CuentaModel> Cuentas { get; set; }
        public DbSet<MovimientoModel> Movimientos { get; set; }
    }
}
