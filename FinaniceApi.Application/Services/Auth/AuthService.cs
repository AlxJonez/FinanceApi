using FinanceApi.Infrastructure.Entity.Auth;
using FinaniceApi.Application.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaniceApi.Application.Services.Auth
{
    public class AuthService
    {
        private readonly Dictionary<SignType, IAuth> _providers;

        public AuthService(IEnumerable<IAuth> providers)
        {
            _providers = new Dictionary<SignType, IAuth>();

            foreach (var provider in providers)
            {
                _providers[provider.Type] = provider;
            }
        }

        public Task<AuthResponse> SignInAsync(SignRequest request)
        {
            if (!_providers.TryGetValue(request.Type, out var authProvider))
            {
                throw new InvalidOperationException($"Auth provider not found for type: {request.Type}");
            }

            return authProvider.SignInAsync(request);
        }
    }
}
