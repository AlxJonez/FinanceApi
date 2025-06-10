using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure.Database.Options
{
    public class MongoDBOptions
    {
        private const string SECTION = "MongoDB";
        /// <summary>
        /// DB name
        /// </summary>
        public string DatabaseName { get; set; } = null!;
        public string AuthSource { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
        public SecureString SecurePassword { get; private set; }

        public MongoDBOptions(IConfiguration configuration, string key = SECTION)
        {
            IConfigurationSection section = configuration.GetSection(key);
            section.Bind(this);

            if (string.IsNullOrEmpty(UserName))
                throw new Exception($"{SECTION}:{nameof(UserName)} not exist");

            if (string.IsNullOrEmpty(Password))
                throw new Exception($"{SECTION}:{nameof(Password)} not exist");


            SecurePassword = new SecureString();
            foreach (char c in Password)
            {
                SecurePassword.AppendChar(c);
            }
            SecurePassword.MakeReadOnly();
        }
    }
}
