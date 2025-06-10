using FinanceApi.Infrastructure.Entity.Auth;
using FinaniceApi.Application.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaniceApi.Application.Services.Auth
{
    public class AppleAuthService : IAuth
    {
        public SignType Type => SignType.Apple;

        public Task<AuthResponse> SignInAsync(SignRequest request)
        {
            // Валидация Apple ID token через Apple API
            return Task.FromResult(new AuthResponse
            {
                AccessToken = "mock_access_token_apple",
                RefreshToken = "mock_refresh_token"
            });
        }
    }
}
