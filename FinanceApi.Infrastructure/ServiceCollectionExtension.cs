using FinanceApi.Infrastructure.Abstraction;
using FinanceApi.Infrastructure.Database.Options;
using FinanceApi.Infrastructure.Database.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new MongoDBOptions(configuration);
            var settings = MongoClientSettings.FromConnectionString(options.ConnectionString);
            settings.Credential = MongoCredential.CreateCredential(options.AuthSource, options.UserName, options.SecurePassword);
            var mongoClient = new MongoClient(settings);
            services.AddSingleton(mongoClient.GetDatabase(options.DatabaseName));

            services.AddScoped<IUserRepository, UserMongoRepository>();
            services.AddScoped<ICategoryRepository, CategoryMongoRepository>();
            services.AddScoped<ITransactionRepository, TransactionMongoRepository>();
            services.AddScoped<ISpaceRepository, SpaceMongoRepository>();
            services.AddScoped<ISpaceUserRepository, SpaceUserMongoRepository>();
            services.AddScoped<ICategoryRepository, CategoryMongoRepository>();
            services.AddScoped<IBudgetRepository, BudgetMongoRepository>();
            return services;
        }
    }
}
