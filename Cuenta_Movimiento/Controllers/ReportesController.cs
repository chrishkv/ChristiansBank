using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Cuenta_Movimiento.Models.Data;
using System.Text.Json;

namespace Cuenta_Movimiento.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly DBContext _dbContext;
        private readonly MyApiClient _apiClient;

        public ReportesController(DBContext dbContext, MyApiClient apiClient)
        {
            _dbContext = dbContext;
            _apiClient = apiClient;
        }

        // GET: reportes/
        [HttpGet]
        public async Task<ActionResult<string>> GetReporte(DateTime fechaInicio, DateTime fechaFin, int cuentaId)
        {
            List<MovimientoModel> movimientos = await _dbContext.Movimientos
                .Include(m => m.cuenta)
                .Where(m => m.fecha >= fechaInicio && m.fecha <= fechaFin && m.cuenta.cuenta_id == cuentaId)
                .ToListAsync();
            if (movimientos.Any())
            {
                var cliente = await _apiClient.MakeRequestAPI($"http://localhost:80/Clientes/{movimientos[0].cuenta.cliente_id}");
                JsonDocument jsonDocument = JsonDocument.Parse(cliente);
                string nombre_cliente = jsonDocument.RootElement.GetProperty("persona").GetProperty("nombre").GetString();
                List<object> result = new List<object>();
                foreach (MovimientoModel movimiento in movimientos)
                {
                    result.Add(new
                    {
                        Fecha = movimiento.fecha,
                        Cliente = nombre_cliente,
                        Numero_Cuenta = movimiento.cuenta.numero_cuenta,
                        Tipo = movimiento.cuenta.tipo_cuenta,
                        Saldo_Inicial = movimiento.cuenta.saldo_inicial,
                        Estado = movimiento.cuenta.estado,
                        Movimiento = movimiento.valor,
                        Saldo_Disponible = movimiento.saldo,
                    });
                }

                return Ok(JsonConvert.SerializeObject(result));
            }

            return NotFound();
        }
    }
}
