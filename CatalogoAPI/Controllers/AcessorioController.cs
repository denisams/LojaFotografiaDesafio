using System.Threading.Tasks;
using Catalogo.Core.Entities;
using Catalogo.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcessorioController : ControllerBase
    {
        private readonly IAccessoryRepository _accessoryRepository;

        public AcessorioController(IAccessoryRepository accessoryRepository)
        {
            _accessoryRepository = accessoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var accessory = await _accessoryRepository.GetAllAsync();
            return Ok(accessory);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var accessory = await _accessoryRepository.GetByIdAsync(id);
            if (accessory == null)
            {
                return NotFound();
            }
            return Ok(accessory);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Accessory accessory)
        {
            int newId = await _accessoryRepository.GetNextIdAsync();
            accessory.Id = newId;

            await _accessoryRepository.AddAsync(accessory);
            return CreatedAtAction(nameof(Get), new { id = accessory.Id }, accessory);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Accessory accessory)
        {
            var existingAcessorio = await _accessoryRepository.GetByIdAsync(id);
            if (existingAcessorio == null)
            {
                return NotFound();
            }

            accessory.Id = id; // Garantir que o ID do acessório a ser atualizado corresponda ao ID fornecido
            await _accessoryRepository.UpdateAsync(accessory);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var acessorio = await _accessoryRepository.GetByIdAsync(id);
            if (acessorio == null)
            {
                return NotFound();
            }
            await _accessoryRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
