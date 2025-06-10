using FinanceApi.Infrastructure.Entity.CoreModels;
using FinaniceApi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FinanceApi.Controllers
{
    [ApiController]
    [Route("api/spaces")]
    [Authorize]
    public class SpaceController : Controller
    {

        private readonly SpaceService _spaceService;
        public SpaceController(SpaceService spaceService)
        {
            _spaceService = spaceService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateSpaceDto dto)
        {
            if (!Enum.TryParse<Currency>(dto.Currency, true, out var parsedCurrency))
                return BadRequest($"Валюта {dto.Currency} не поддерживается для использования.");
            Guid userId = GetUserId();
            var space = await _spaceService.CreateSpace(userId, dto.Name, parsedCurrency);
            return Ok(new { message = "Пространство успешно создано", space });
        }

        [HttpGet("get-my")]
        [SwaggerResponse(200, type: typeof(List<Space>))]
        public async Task<IActionResult> GetAll()
        {
            Guid userId = GetUserId();
            var spaces = await _spaceService.GetUserSpaces(userId);
            return Ok(spaces);
        }

        [HttpDelete("{spaceId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid spaceId, [FromQuery] bool softDelete = true)
        {
            Guid userId = GetUserId();
            var isDeleted = await _spaceService.Delete(userId, spaceId, softDelete);
            if (isDeleted)
                return Ok(new { message = "Пространство успешно удалено" });
            return BadRequest("Не удалось удалить пространство.");
        }


        [HttpPatch("{spaceId}")]
        public async Task<IActionResult> Update([FromBody] Space spaceDto)
        {
            Guid userId = GetUserId();
            var updatedSpace = await _spaceService.Update(userId, spaceDto);
            if (updatedSpace is not null)
                return Ok(new { message = "Пространство обновлено", space = updatedSpace });
            return BadRequest("Не удалось обновить пространство.");
        }

        private Guid GetUserId()
        {
            string userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userid")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                throw new Exception("Не удалось узнать пользователя!");
            return userId;
        }
    }

    public class CreateSpaceDto
    {
        public string Name { get; set; }
        public string Currency { get; set; }
    }
}
