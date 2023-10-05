using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Cliente.Models.Data;
using Microsoft.AspNetCore.JsonPatch;

namespace Cliente.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly DBContext _dbContext;

        public ClientesController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: clientes/
        [HttpGet]
        public async Task<ActionResult<string>> GetClientes()
        {
            List<ClienteModel> clientes = await _dbContext.Set<ClienteModel>().ToListAsync();

            return Ok(JsonConvert.SerializeObject(clientes));
        }

        // GET clientes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteModel>> GetCliente(int id)
        {
            var cliente = await _dbContext.Clientes.Include(c => c.persona).FirstOrDefaultAsync(c => c.clienteid == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // POST clientes/
        [HttpPost]
        public async Task<ActionResult<ClienteModel>> CreateCliente(ClienteModel cliente)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Clientes.Add(cliente);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCliente), new { id = cliente.clienteid }, cliente);
            } else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT clientes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, ClienteModel cliente)
        {
            if (id != cliente.clienteid) return BadRequest();

            if (ModelState.IsValid)
            {
                _dbContext.Entry(cliente).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCliente), new { id = cliente.clienteid }, cliente);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE clientes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                var cliente = await _dbContext.Clientes.FindAsync(id);
                if (cliente == null) return NotFound();

                _dbContext.Clientes.Remove(cliente);
                await _dbContext.SaveChangesAsync();

                return Ok("Cliente eliminado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la persona: {ex.Message}");
            }
        }

        // PATCH Clientes/{id}
        [HttpPatch("{id}")]
        public IActionResult UpdateCliente(int id, [FromBody] JsonPatchDocument<ClienteModel> clintePatch)
        {
            var cliente = _dbContext.Clientes.Include(c => c.persona).FirstOrDefault(c => c.clienteid == id);
            if (cliente == null) return NotFound();

            clintePatch.ApplyTo(cliente);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _dbContext.SaveChanges();

            return Ok(cliente);
        }

    }
}