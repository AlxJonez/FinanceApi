using FinanceApi.Infrastructure.Entity.CoreModels;
using FinaniceApi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FinanceApi.Controllers
{
    public class UserController : Controller
    {
        [ApiController]
        [Route("api/users")]
        public class UsersController : ControllerBase
        {
            private readonly UserService _userService;

            public UsersController(UserService userService)
            {
                _userService = userService;
            }

            [Authorize(Roles = "Admin")]
            [HttpPost("create")]
            public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
            {
                var user = await _userService.CreateNew(dto.SignType, dto.Identify, dto.Password, dto.NickName);
                return Ok(new { message = "User created successfully", user });
            }

            [Authorize(Roles = "Admin")]
            [HttpGet]
            [SwaggerResponse(200, type: typeof(List<User>))]
            public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
            {
                int skip = (page - 1) * pageSize;
                var users = await _userService.GetUsersAsync(skip, pageSize);
                return Ok(users);
            }

            public class UserCreateDto
            {
                public string SignType { get; set; }
                public string Identify { get; set; }
                public string Password { get; set; }
                public string NickName { get; set; }
            }
        }
    }
}
