using FinanceApi.Infrastructure.Entity.CoreModels;
using FinaniceApi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceApi.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionsController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Transaction transaction)
        {
            //валидация UserId из куков и запроса

            transaction.UserId = Guid.Parse(User.FindFirstValue("userid"));
            var result = await _transactionService.CreateAsync(transaction);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Transaction transaction)
        {
            var result = await _transactionService.UpdateAsync(transaction);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _transactionService.DeleteAsync(id);
            return Ok(new { deleted = result });
        }

        [HttpGet("history")]
        public async Task<IActionResult> History([FromQuery] DateTime from, [FromQuery] DateTime to, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var userId = Guid.Parse(User.FindFirstValue("userid"));
            int skip = (page - 1) * pageSize;
            var result = await _transactionService.GetAllByPeriodAsync(userId, from, to, skip, pageSize);
            return Ok(result);
        }

        [HttpGet("last")]
        public async Task<IActionResult> GetLast([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var userId = Guid.Parse(User.FindFirstValue("userid"));
            int offset = pageSize * (page - 1);
            var transactions = await _transactionService.GetLast(userId, offset, pageSize);
            return Ok(transactions);
        }
    }
}
