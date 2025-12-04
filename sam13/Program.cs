using Polly;
using Polly.Timeout;
using System;
using System.Net.Http;
using System.Threading;

namespace IndependentWork13
{
    internal class Program
    {
        private static int _apiAttempts = 0;     // Лічильник спроб API
        private static int _dbFailures = 0;      // Лічильник помилок БД

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Самостійна робота №13 – Дослідження Polly\n");

            Scenario1_RetryExternalApi();
            Console.WriteLine();
            Scenario2_CircuitBreakerDatabase();
            Console.WriteLine();
            Scenario3_TimeoutOperation();

            Console.WriteLine("\nУсі сценарії виконано.");
        }

        // ---------------------------------------------------------
        // СЦЕНАРІЙ 1: Виклик зовнішнього API + Retry Policy
        // ---------------------------------------------------------

        // Імітація зовнішнього API, який двічі повертає помилку
        public static string CallExternalApi()
        {
            _apiAttempts++;
            Console.WriteLine($"Спроба {_apiAttempts}: звернення до зовнішнього API...");

            if (_apiAttempts <= 2)
                throw new HttpRequestException("Тимчасова помилка API");

            Console.WriteLine("API успішно повернув дані.");
            return "Отримані дані API";
        }

        public static void Scenario1_RetryExternalApi()
        {
            Console.WriteLine("=== Сценарій 1: Повторні спроби (Retry) при виклику зовнішнього API ===");

            // Політика повторних спроб з експоненційною затримкою
            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetry(
                    3,                                                      // максимум 3 повтори
                    attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),  // затримки: 2, 4, 8 секунд
                    (exception, timespan, retry, context) =>
                    {
                        Console.WriteLine(
                            $"Повторна спроба {retry} через {timespan.TotalSeconds} сек. Причина: {exception.Message}");
                    });

            try
            {
                string result = retryPolicy.Execute(() => CallExternalApi());
                Console.WriteLine($"Фінальний результат: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Операція не вдалася після всіх повторів: {ex.Message}");
            }
        }

        // ---------------------------------------------------------
        // СЦЕНАРІЙ 2: Проблеми з підключенням до БД + Circuit Breaker
        // ---------------------------------------------------------

        // Імітація підключення до бази даних
        public static void ConnectToDatabase()
        {
            _dbFailures++;
            Console.WriteLine($"Спроба підключення до БД №{_dbFailures}");

            if (_dbFailures <= 3)
                throw new Exception("Помилка підключення до бази даних");

            Console.WriteLine("База даних успішно підключена.");
        }

        public static void Scenario2_CircuitBreakerDatabase()
        {
            Console.WriteLine("=== Сценарій 2: Circuit Breaker для підключення до бази даних ===");

            // Політика Circuit Breaker
            var breaker = Policy
                .Handle<Exception>()
                .CircuitBreaker(
                    2,                          // після 2 помилок підряд
                    TimeSpan.FromSeconds(5),    // "відкритий" стан триватиме 5 секунд
                    (ex, ts) =>
                    {
                        Console.WriteLine($"Схема відкрита на {ts.TotalSeconds} секунд. Причина: {ex.Message}");
                    },
                    () =>
                    {
                        Console.WriteLine("Схема закрита. Робота відновлена.");
                    });

            for (int i = 1; i <= 6; i++)
            {
                try
                {
                    breaker.Execute(() => ConnectToDatabase());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Операція заблокована: {ex.Message}");
                }

                Thread.Sleep(1000); // очікування між викликами
            }
        }

        // ---------------------------------------------------------
        // СЦЕНАРІЙ 3: Довга операція + Timeout Policy
        // ---------------------------------------------------------

        // Імітація довгої операції (4 секунди)
        public static void LongOperation()
        {
            Console.WriteLine("Початок довгої операції...");
            Thread.Sleep(4000);
        }

        public static void Scenario3_TimeoutOperation()
        {
            Console.WriteLine("=== Сценарій 3: Таймаут (Timeout Policy) для довгої операції ===");

            // Політика таймауту на 2 секунди
            var timeout = Policy
                .Timeout(
                    2,
                    TimeoutStrategy.Pessimistic,
                    (context, ts, task) =>
                    {
                        Console.WriteLine($"Досягнуто таймаут {ts.TotalSeconds} секунд.");
                    });

            try
            {
                timeout.Execute(() => LongOperation());
            }
            catch (TimeoutRejectedException)
            {
                Console.WriteLine("Операцію перервано через перевищення допустимого часу.");
            }
        }
    }
}