using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure.Entity.Auth
{
    public class SignRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SignType Type { get; set; }
        public string Identifier { get; set; } // email, phone и т.д.
        public string? Password { get; set; }
    }
}
