using FinanceApi.Infrastructure.Entity.Auth;
using FinaniceApi.Application.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaniceApi.Application.Services.Auth
{
    public class GoogleAuthService : IAuth
    {
        public SignType Type => SignType.Google;

        public Task<AuthResponse> SignInAsync(SignRequest request)
        {
            // Валидация Google ID token через Google API
            return Task.FromResult(new AuthResponse
            {
                AccessToken = "mock_access_token_google",
                RefreshToken = "mock_refresh_token"
            });
        }
    }
}
