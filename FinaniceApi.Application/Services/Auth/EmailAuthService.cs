using FinanceApi.Infrastructure.Entity.Auth;
using FinaniceApi.Application.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaniceApi.Application.Services.Auth
{
    public class EmailAuthService : IAuth
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly UserService _userService;

        public EmailAuthService(JwtTokenService jwtTokenService, UserService userService)
        {
            _jwtTokenService = jwtTokenService;
            _userService = userService;
        }


        public SignType Type => SignType.Email;

        public async Task<AuthResponse> SignInAsync(SignRequest request)
        {
            var user = await _userService.GetByEmailAsync(request.Identifier);
            if (user is null)
                throw new Exception($"User '{request.Identifier}' not existed");
            if (CreateMD5(request.Password) != user.PasswordHash)
            {
                throw new Exception($"Password incorrect!");
            }

            var accessToken = _jwtTokenService.GenerateAccessToken(user.Id, user.Name, user.Status);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id
            };
        }

        private static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes);
            }
        }
    }
}
