using FinanceApi.Infrastructure.Entity.Auth;
using FinaniceApi.Application.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaniceApi.Application.Services.Auth
{
    public class PhoneAuthService : IAuth
    {
        public SignType Type => SignType.Phone;

        public Task<AuthResponse> SignInAsync(SignRequest request)
        {
            // Здесь: проверка OTP-кода или phone токена
            return Task.FromResult(new AuthResponse
            {
                AccessToken = "mock_access_token_phone",
                RefreshToken = "mock_refresh_token"
            });
        }
    }
}
