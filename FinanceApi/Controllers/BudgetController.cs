using FinanceApi.Infrastructure.Entity.CoreModels;
using FinaniceApi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FinanceApi.Controllers
{
    [ApiController]
    [Route("api/budgets")]
    [Authorize]
    public class BudgetsController : ControllerBase
    {
        private readonly BudgetService _budgetService;

        public BudgetsController(BudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateBudgetDto dto)
        {
            var budget = await _budgetService.Create(dto.SpaceId, dto.Currency, dto.Name, dto.ParentId);
            return Ok(budget);
        }

        [SwaggerResponse(200, type: typeof(List<Budget>))]
        [HttpGet("space/{spaceId}")]
        public async Task<IActionResult> GetMy(Guid spaceId)
        {

            var result = await _budgetService.GetSpaceBudgets(spaceId);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Budget updatedBudget)
        {
            var userId = GetUserId();
            var result = await _budgetService.Update(userId, updatedBudget);
            return Ok(result);
        }

        [HttpDelete("{spaceId}/{budgetId}")]
        public async Task<IActionResult> Delete(Guid spaceId, Guid budgetId)
        {
            var userId = GetUserId();
            var result = await _budgetService.Delete(userId, spaceId, budgetId);
            return Ok(new { deleted = result });
        }

        private Guid GetUserId()
        {
            string userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userid")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                throw new Exception("Не удалось узнать пользователя!");
            return userId;
        }

        public class CreateBudgetDto
        {
            public Guid SpaceId { get; set; }
            public Currency Currency { get; set; }
            public string Name { get; set; }
            public Guid? ParentId { get; set; }
        }
    }
}
