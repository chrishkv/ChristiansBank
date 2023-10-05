using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Cuenta_Movimiento.Models.Data;
using Microsoft.AspNetCore.JsonPatch;

namespace Cuenta_Movimiento.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly DBContext _dbContext;

        public CuentasController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Cuentas/
        [HttpGet]
        public async Task<ActionResult<string>> GetCuentas()
        {
            List<CuentaModel> cuentas = await _dbContext.Set<CuentaModel>().ToListAsync();

            return Ok(JsonConvert.SerializeObject(cuentas));
        }

        // GET Cuentas/{numero_cuenta}
        [HttpGet("{numero_cuenta}")]
        public async Task<ActionResult<CuentaModel>> GetCuenta(int numero_cuenta)
        {
            CuentaModel cuenta = await _dbContext.Cuentas.FirstOrDefaultAsync(c => c.numero_cuenta == numero_cuenta);
            if (cuenta == null) return NotFound();

            return cuenta;
        }

        // POST Cuentas/
        [HttpPost]
        public async Task<ActionResult<CuentaModel>> PostCuenta([FromBody] CuentaModel cuenta)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Cuentas.Add(cuenta);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCuenta), new { numero_cuenta = cuenta.numero_cuenta }, cuenta);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT Cuentas/{numero_cuenta}
        [HttpPut("{numero_cuenta}")]
        public async Task<IActionResult> PutCuenta(int numero_cuenta, [FromBody] CuentaModel cuenta)
        {
            if (numero_cuenta != cuenta.numero_cuenta) return BadRequest();

            if (ModelState.IsValid)
            {
                CuentaModel cuentafinded = await _dbContext.Cuentas.FirstOrDefaultAsync(c => c.numero_cuenta == numero_cuenta);
                if (cuentafinded != null)
                {
                    cuentafinded.numero_cuenta = cuenta.numero_cuenta;
                    cuentafinded.tipo_cuenta = cuenta.tipo_cuenta;
                    cuentafinded.saldo_inicial = cuenta.saldo_inicial;
                    cuentafinded.estado = cuenta.estado;
                    cuentafinded.cliente_id = cuenta.cliente_id;
                    _dbContext.Entry(cuentafinded).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }

                return CreatedAtAction(nameof(GetCuenta), new { numero_cuenta = cuenta.numero_cuenta }, cuenta);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE Cuentas/{numero_cuenta}
        [HttpDelete("{numero_cuenta}")]
        public async Task<IActionResult> DeleteCuenta(int numero_cuenta)
        {
            try
            {
                CuentaModel cuenta = await _dbContext.Cuentas.FirstOrDefaultAsync(c => c.numero_cuenta == numero_cuenta);
                if (cuenta == null) return NotFound();

                _dbContext.Cuentas.Remove(cuenta);
                await _dbContext.SaveChangesAsync();

                return Ok("Cuenta eliminada correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la cuenta: {ex.Message}");
            }
        }

        // PATCH Cuentas/{numero_cuenta}
        [HttpPatch("{numero_cuenta}")]
        public async Task<IActionResult> UpdateCuenta(int numero_cuenta, [FromBody] JsonPatchDocument<CuentaModel> patchDocument)
        {
            CuentaModel existingCuenta = _dbContext.Cuentas.FirstOrDefault(c => c.numero_cuenta == numero_cuenta);
            if (existingCuenta == null)
            {
                return NotFound();
            }
            patchDocument.ApplyTo(existingCuenta);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _dbContext.SaveChangesAsync();

            return Ok(existingCuenta);
        }
    }
}
