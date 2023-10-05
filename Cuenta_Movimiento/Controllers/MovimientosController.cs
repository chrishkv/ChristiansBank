using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Cuenta_Movimiento.Models.Data;
using Microsoft.AspNetCore.JsonPatch;

namespace Cuenta_Movimiento.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly DBContext _dbContext;

        public MovimientosController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Movimientos/
        [HttpGet]
        public async Task<ActionResult<string>> GetMovimientos()
        {
            List<MovimientoModel> movimientos = await _dbContext.Set<MovimientoModel>().ToListAsync();

            return Ok(JsonConvert.SerializeObject(movimientos));
        }

        // GET Movimientos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MovimientoModel>> GetMovimiento(int movimiento_id)
        {
            MovimientoModel movimiento = await _dbContext.Movimientos.Include(c => c.cuenta).FirstOrDefaultAsync(c => c.movimiento_id == movimiento_id);

            if (movimiento == null)
            {
                return NotFound();
            }

            return movimiento;
        }

        // POST Movimientos/
        [HttpPost]
        public async Task<ActionResult<MovimientoModel>> CreateMovimiento(MovimientoModel movimiento)
        {
            var cuentaExistente = await _dbContext.Cuentas.FirstOrDefaultAsync(c => c.numero_cuenta == movimiento.cuenta.numero_cuenta);

            if (cuentaExistente == null)
            {
                if ((movimiento.cuenta.saldo_inicial + movimiento.valor) >= 0)
                {
                    movimiento.cuenta.saldo_inicial += movimiento.valor;
                    movimiento.saldo = movimiento.cuenta.saldo_inicial;
                    _dbContext.Movimientos.Add(movimiento);
                    await _dbContext.SaveChangesAsync();
                } else
                {
                    return StatusCode(500, "Saldo no disponible");
                }
            }
            else
            {
                if ((cuentaExistente.saldo_inicial + movimiento.valor) >= 0)
                {
                    cuentaExistente.saldo_inicial += movimiento.valor;
                    movimiento.cuenta = cuentaExistente;
                    movimiento.saldo = cuentaExistente.saldo_inicial;
                    _dbContext.Movimientos.Update(movimiento);
                }
                else
                {
                    return StatusCode(500, "Saldo no disponible");
                }
            }

            return Ok(GetMovimientoResult(movimiento.movimiento_id));
        }


        // PUT Movimientos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, MovimientoModel movimiento)
        {
            if (id != movimiento.movimiento_id) return BadRequest();

            if (ModelState.IsValid)
            {
                _dbContext.Entry(movimiento).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetMovimiento), new { id = movimiento.movimiento_id }, movimiento);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE Movimientos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                MovimientoModel movimiento = await _dbContext.Movimientos.FindAsync(id);
                if (movimiento == null) return NotFound();

                _dbContext.Movimientos.Remove(movimiento);
                await _dbContext.SaveChangesAsync();

                return Ok("Movimiento eliminado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el movimiento: {ex.Message}");
            }
        }

        // PATCH Movimientos/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] JsonPatchDocument<MovimientoModel> clintePatch)
        {
            MovimientoModel movimiento = _dbContext.Movimientos.Include(c => c.cuenta).FirstOrDefault(c => c.movimiento_id == id);
            if (movimiento == null) return NotFound();

            clintePatch.ApplyTo(movimiento);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _dbContext.SaveChangesAsync();

            return Ok(movimiento);
        }

        private async Task<ActionResult<MovimientoModel>> GetMovimientoResult(int movimiento_id)
        {
            var movimiento = await _dbContext.Movimientos.FindAsync(movimiento_id);
            if (movimiento == null) return NotFound();
            return movimiento;
        }
    }
}
