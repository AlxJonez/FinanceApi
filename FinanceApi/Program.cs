using FinanceApi.Infrastructure;
using FinaniceApi.Application;
using Microsoft.AspNetCore.Builder;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

//забираем текущее окружение, для работы с дев конфигом
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

// Настраиваем конфигурацию
var builderConfig = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var configuration = builderConfig.Build();

//подключаем DI от слоев апп и инфрастракт
builder.Services.AddApplication(configuration);
builder.Services.AddInfrastructure(configuration);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Введите токен в формате: Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
//для поддержки общения фронта с беком
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Configure(configuration.GetSection("Kestrel"));
    //options.ListenAnyIP(5003, listenOptions =>
    //{
    //    listenOptions.UseHttps(configuration["Settings:CertificatePath"], configuration["Settings:CertificatePassword"]);
    //});
    options.ListenAnyIP(5004);
});


var app = builder.Build();

//включаем сваггер
app.UseSwagger();

app.UseCors("AllowAll");
//app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
