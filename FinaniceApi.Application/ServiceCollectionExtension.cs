using FinaniceApi.Application.Abstraction;
using FinaniceApi.Application.Services;
using FinaniceApi.Application.Services.Auth;
using FinaniceApi.Application.Services.ChatGPT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaniceApi.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuth(configuration);

            services.AddScoped<UserService>();
            services.AddScoped<SpaceService>();
            services.AddScoped<BudgetService>();
            services.AddScoped<TransactionService>();
            services.AddScoped<MoneyConverterService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<ReceiptClassifierService>();
            return services;
        }
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                //пока используется стандартную через Authorization: Bearer Token
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]))
                };
            });

            //сервис генерации токена+рефреш токена
            services.AddSingleton<JwtTokenService>();

            services.AddAuthorization();

            //Сервисы авторизации по типу входа
            services.AddScoped<IAuth, EmailAuthService>();
            services.AddScoped<IAuth, PhoneAuthService>();
            services.AddScoped<IAuth, GoogleAuthService>();
            services.AddScoped<IAuth, AppleAuthService>();
            services.AddScoped<AuthService>();

            return services;
        }
    }
}
