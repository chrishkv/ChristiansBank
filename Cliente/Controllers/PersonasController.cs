using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Cliente.Models.Data;
using Microsoft.AspNetCore.JsonPatch;

namespace Cliente_Persona.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly DBContext _dbContext;

        public PersonasController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Personas/
        [HttpGet]
        public async Task<ActionResult<string>> GetPersonas()
        {
            List<PersonaModel> personas = await _dbContext.Set<PersonaModel>().ToListAsync();

            return Ok(JsonConvert.SerializeObject(personas));
        }

        // GET Personas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonaModel>> GetPersona(int id)
        {
            var persona = await _dbContext.Personas.FindAsync(id);
            if (persona == null) return NotFound();

            return persona;
        }

        // POST Personas/
        [HttpPost]
        public async Task<ActionResult<PersonaModel>> PostPersona([FromBody] PersonaModel persona)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Personas.Add(persona);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPersona), new { id = persona.Id }, persona);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT Personas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersona(int id, [FromBody] PersonaModel persona)
        {
            if (id != persona.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                _dbContext.Entry(persona).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPersona), new { id = persona.Id }, persona);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        // DELETE Personas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersona(int id)
        {
            try
            {
                var persona = await _dbContext.Personas.FindAsync(id);
                if (persona == null) return NotFound();

                _dbContext.Personas.Remove(persona);
                await _dbContext.SaveChangesAsync();

                return Ok("Persona eliminada correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la persona: {ex.Message}");
            }
        }

        // PATCH Personas/{id}
        [HttpPatch("{id}")]
        public IActionResult UpdatePersona(int id, [FromBody] JsonPatchDocument<PersonaModel> personaPatch)
        {
            var persona = _dbContext.Personas.FirstOrDefault(p => p.Id == id);
            if (persona == null) return NotFound();

            personaPatch.ApplyTo(persona);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _dbContext.SaveChanges();

            return Ok(persona);
        }

    }
}
