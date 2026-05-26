using System;
using System.Collections.Generic;

namespace lab30vN
{
    public class CurrencyConverter
    {
        private Dictionary<string, decimal> rates = new Dictionary<string, decimal>()
        {
            {"USD", 1},
            {"EUR", 0.9m},
            {"UAH", 40}
        };

        public decimal GetRate(string currency)
        {
            if (!rates.ContainsKey(currency))
                throw new ArgumentException("Валюта не підтримується");

            return rates[currency];
        }

        public decimal Convert(string from, string to, decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Сума повинна бути додатною");

            decimal fromRate = GetRate(from);
            decimal toRate = GetRate(to);

            decimal usd = amount / fromRate;
            return usd * toRate;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Конвертер валют");
        }
    }
}