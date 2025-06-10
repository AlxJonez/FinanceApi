using FinanceApi.Infrastructure.Entity.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaniceApi.Application.Abstraction
{
    /// <summary>
    /// Общий интерфейс для всех провайдеров авторизации
    /// </summary>
    public interface IAuth
    {
        SignType Type { get; }
        Task<AuthResponse> SignInAsync(SignRequest request);
    }
}
