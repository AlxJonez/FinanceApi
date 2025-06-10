using FinaniceApi.Application.Services.ChatGPT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Controllers
{
    [ApiController]
    [Route("api/ChatGPT")]
    [Authorize]
    public class ChatGPTController : Controller
    {
        ReceiptClassifierService _receiptClassifierService { get; set; }
        public ChatGPTController(ReceiptClassifierService receiptClassifierService)
        {
            _receiptClassifierService = receiptClassifierService;
        }

        [HttpPost("receipt")]
        public async Task<IActionResult> ProcessReceipt([FromBody] string ocr)
        {
            var result = await _receiptClassifierService.Parse(ocr);
            return Ok(result);
        }

    }
}
