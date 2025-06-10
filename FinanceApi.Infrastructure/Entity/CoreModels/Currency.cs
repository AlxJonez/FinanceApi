using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure.Entity.CoreModels
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Currency
    {
        UAH, //будет по умолчанию если не указано явно
        USD,
        RUR,
        EUR
    }
}
