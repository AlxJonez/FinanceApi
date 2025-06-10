using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure.Entity.CoreModels.ChatGPT
{
    public class ChatGptRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = "gpt-4";
        [JsonPropertyName("messages")]
        public ChatGptMessage[] Messages { get; set; }
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; } = 0.2;
    }
}
