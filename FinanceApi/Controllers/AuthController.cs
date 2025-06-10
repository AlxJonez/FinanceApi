using FinanceApi.Infrastructure.Entity.Auth;
using FinaniceApi.Application.Services;
using FinaniceApi.Application.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace FinanceApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly JwtTokenService _jwtTokenService;
        private readonly UserService _userService;

        public AuthController(AuthService authService, JwtTokenService jwtTokenService, UserService userService)
        {
            _userService = userService;
            _authService = authService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        [SwaggerResponse(200, type: typeof(AuthResponse))]
        public async Task<IActionResult> SignIn([FromBody] SignRequest request)
        {
            try
            {
                var result = await _authService.SignInAsync(request);

                // Сохраняем refresh токен в cookie (HttpOnly)
                Response.Cookies.Append("refresh_token", result.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

                return Ok(new AuthResponse()
                {
                    AccessToken = result.AccessToken,
                    UserId = result.UserId,
                    Succes = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("refresh_token");
            return Ok(new { message = "Logged out" });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirstValue("userid");
            if (string.IsNullOrEmpty(userId))
                return Forbid();
            var nickname = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var profileSummary = await _userService.GetProfile(Guid.Parse(userId));
            return Ok(profileSummary);
        }
    }

    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public bool Succes { get; set; }
        public string? Details { get; set; }

        public Guid? UserId { get; set; }
        public AuthResponse() { }
        public AuthResponse(string accessToken)
        {
            AccessToken = accessToken;
        }

        public override bool Equals(object? obj)
        {
            return obj is AuthResponse other &&
                   AccessToken == other.AccessToken;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AccessToken);
        }
    }

}
