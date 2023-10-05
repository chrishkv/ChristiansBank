using Microsoft.EntityFrameworkCore;

namespace Cliente.Models.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public DbSet<PersonaModel> Personas { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }
    }
}
