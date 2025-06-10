using FinanceApi.Infrastructure.Entity.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaniceApi.Application.Services
{
    public class MoneyConverterService
    {
        private readonly Dictionary<Currency, decimal> _ratesToUsd = new()
    {
        { Currency.USD, 1.0m },
        { Currency.UAH, 0.024m },
        { Currency.EUR, 1.12346m },
        { Currency.RUR, 0.0119m }
    };
        public MoneyConverterService() { }

        public async Task<decimal> Convert(Currency from, Currency to, decimal value)
        {
            if (from == to) return value;

            if (!_ratesToUsd.TryGetValue(from, out var fromRate))
                throw new Exception($"Unknown rate for currency: {from}");
            if (!_ratesToUsd.TryGetValue(to, out var toRate))
                throw new Exception($"Unknown rate for currency: {to}");
            var usdValue = value * fromRate;
            var result = usdValue / toRate;

            return Math.Round(result, 2);
        }
    }
}
