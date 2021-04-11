using System.Collections.Generic;

namespace Wallets.BusinessLayer
{
    public enum Currency
    {
        USD,
        EUR,
        UAH,
        RUB
    };

    public static class Converter
    {
        private static Dictionary<Currency, decimal> _convert = new Dictionary<Currency, decimal>
        {
            [Currency.USD] = 1m,
            [Currency.EUR] = 0.84m,
            [Currency.UAH] = 27.9m,
            [Currency.RUB] = 77.43m
        };

        public static Dictionary<Currency, decimal> Convert { get => _convert; }

        public static decimal СomputeTheCoefficient(Currency? currencyFrom, Currency? currencyTo)
        {
            decimal from = Converter.Convert[currencyFrom.Value];
            decimal to = Converter.Convert[currencyTo.Value];
            return to / from;
        }

    }

}
