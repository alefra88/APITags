using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TagAPI.DTO;
using TagAPI.Services.Interfaces;
using TagAPI.Extensions;

namespace TagAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarTag([FromBody] TagCreateRequestDTO request)
        {
            // Validación y persistencia
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //servicio procesa la regla de negocio
            var resultado = await _tagService.RegistrarNuevoTagAsync(request);

            // transforma la entidad en un DTO
            var response = resultado.ToCreatedResponse();

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var tags = await _tagService.ObtenerTodosAsync();
            return Ok(tags.ToDetailResponseList());
        }

        [HttpGet("{numeroTag}")]
        public async Task<IActionResult> ObtenerPorId(string numeroTag)
        {
            var tag = await _tagService.ObtenerPorIdAsync(numeroTag);
            if (tag == null) return NotFound(new { mensaje = $"El TAG {numeroTag} no fue encontrado." });

            return Ok(tag.ToDetailResponse());
        }

        [HttpPut("{numeroTag}")]
        public async Task<IActionResult> ActualizarTag(string numeroTag, [FromBody] TagUpdateRequestDTO request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tagActualizado = await _tagService.ActualizarTagAsync(numeroTag, request);
            if (tagActualizado == null) return NotFound(new { mensaje = $"El TAG {numeroTag} no fue encontrado." });

            return Ok(tagActualizado);
        }

        [HttpDelete("{numeroTag}")]
        public async Task<IActionResult> EliminarTag(string numeroTag)
        {
            var fueEliminado = await _tagService.EliminarTagAsync(numeroTag);
            if (!fueEliminado) return NotFound(new { mensaje = $"El TAG {numeroTag} no fue encontrado." });

            return NoContent(); // Retorna 204 No Content indicando éxito
        }
    }
}