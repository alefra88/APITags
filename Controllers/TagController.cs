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
    }
}