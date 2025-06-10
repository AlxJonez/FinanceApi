using FinanceApi.Infrastructure.Entity.CoreModels;
using FinaniceApi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FinanceApi.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet]
        [SwaggerResponse(200, type: typeof(List<Category>))]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var icons = await _categoryService.GetAllForUserCategories(userId);
            return Ok(icons);
        }

        private Guid GetUserId()
        {
            string userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userid")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                throw new Exception("Не удалось узнать пользователя!");
            return userId;
        }

    }

    public class CategoryCreateDto
    {
        public string CategoryName { get; set; }
        public string? ParentCategoryName { get; set; }
        public string Type { get; set; }

    }
}
