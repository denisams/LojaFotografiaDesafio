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
        private readonly IAcessorioRepository _acessorioRepository;

        public AcessorioController(IAcessorioRepository acessorioRepository)
        {
            _acessorioRepository = acessorioRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var acessorios = await _acessorioRepository.GetAllAsync();
            return Ok(acessorios);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var acessorio = await _acessorioRepository.GetByIdAsync(id);
            if (acessorio == null)
            {
                return NotFound();
            }
            return Ok(acessorio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Acessorio acessorio)
        {
            int newId = await _acessorioRepository.GetNextIdAsync();
            acessorio.Id = newId;

            await _acessorioRepository.AddAsync(acessorio);
            return CreatedAtAction(nameof(Get), new { id = acessorio.Id }, acessorio);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Acessorio acessorio)
        {
            var existingAcessorio = await _acessorioRepository.GetByIdAsync(id);
            if (existingAcessorio == null)
            {
                return NotFound();
            }

            acessorio.Id = id; // Garantir que o ID do acessório a ser atualizado corresponda ao ID fornecido
            await _acessorioRepository.UpdateAsync(acessorio);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var acessorio = await _acessorioRepository.GetByIdAsync(id);
            if (acessorio == null)
            {
                return NotFound();
            }
            await _acessorioRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
