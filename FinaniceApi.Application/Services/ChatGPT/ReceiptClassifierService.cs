using FinanceApi.Infrastructure.Entity.CoreModels;
using FinanceApi.Infrastructure.Entity.CoreModels.ChatGPT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinaniceApi.Application.Services.ChatGPT
{
    public class ReceiptClassifierService
    {
        private readonly string apiKey = "";
        public async Task<Receipt> Parse(string ocrText)
        {
            try
            {
                var request = new ChatGptRequest
                {
                    Messages = new[]
                    {
                    new ChatGptMessage{Role = "system", Content = @"
                                                                    You are a receipt parser. 
                                                                    You will receive OCR-scanned receipt text in different languages. 
                                                                    Translate all data to English and return a valid JSON object matching the structure below.
                                                                        
                                                                    All dates must be in UTC and ISO 8601 format: yyyy-MM-ddTHH:mm:ssZ.        

                                                                    Do not include any explanation or markdown. Respond with JSON only.

                                                                   JSON structure:
                                                                    {
                                                                      ""StoreName"": ""original store name (do not translate)"",
                                                                      ""Address"": ""original store address (do not translate)"",
                                                                      ""Date"": ""purchase date in UTC, ISO format yyyy-MM-ddTHH:mm:ssZ"",
                                                                      ""ReceiptNumber"": ""receipt or transaction number"",
                                                                      ""Discount"": 0.00,
                                                                      ""Total"": 0.00,
                                                                      ""PaymentType"": ""e.g. Cash, Card"",
                                                                      ""Items"": [
                                                                        {
                                                                          ""Name"": ""original product name (do not translate)"",
                                                                          ""Quantity"": 1,
                                                                          ""UnitPrice"": 2.19,
                                                                          ""TotalPrice"": 20.23
                                                                        }
                                                                      ]"},
                    new ChatGptMessage{Role = "user", Content = ocrText }
                }
                };

                var json = JsonSerializer.Serialize(request);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
                var payload = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", payload);

                if (response.IsSuccessStatusCode)
                {
                    using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                    doc.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString()
                        .Trim();
                    return doc.Deserialize<Receipt>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return null;
                }
            }
            catch
            {
                //cant get receipt from ocr
                return null;
            }
        }
    }
}
