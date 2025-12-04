using System;
using System.IO;
using System.Net.Http;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Лабораторна робота №7 ===");
        Console.WriteLine("=== ВАРІАНТ 1 ===");

        var fileProcessor = new FileProcessor();
        var networkClient = new NetworkClient();

        bool ShouldRetry(Exception ex) =>
            ex is FileNotFoundException || ex is HttpRequestException;

        Console.WriteLine("\n--- Тест FileProcessor.ReadFile ---");
        try
        {
            string result = RetryHelper.ExecuteWithRetry(
                () => fileProcessor.ReadFile("data.txt"),
                retryCount: 3,
                initialDelay: TimeSpan.FromMilliseconds(300),
                shouldRetry: ShouldRetry
            );

            Console.WriteLine($"Результат: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Остаточна помилка: {ex.Message}");
        }

        Console.WriteLine("\n--- Тест NetworkClient.DownloadData ---");
        try
        {
            string data = RetryHelper.ExecuteWithRetry(
                () => networkClient.DownloadData("http://example.com"),
                retryCount: 4,
                initialDelay: TimeSpan.FromMilliseconds(300),
                shouldRetry: ShouldRetry
            );

            Console.WriteLine($"Результат: {data}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Остаточна помилка: {ex.Message}");
        }

        Console.WriteLine("\n=== Роботу завершено ===");
    }
}
// =======================
//   RetryHelper
// =======================
public static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool> shouldRetry = null)
    {
        if (initialDelay == default)
            initialDelay = TimeSpan.FromMilliseconds(500);

        int attempt = 0;

        while (true)
        {
            try
            {
                attempt++;
                Console.WriteLine($"Спроба #{attempt}...");
                return operation();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.GetType().Name}: {ex.Message}");

                if (attempt > retryCount || (shouldRetry != null && !shouldRetry(ex)))
                {
                    Console.WriteLine("Повторні спроби завершені.");
                    throw;
                }

                var delay = TimeSpan.FromMilliseconds(initialDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
                Console.WriteLine($"Очікування {delay.TotalMilliseconds} ms...");
                Thread.Sleep(delay);
            }
        }
    }
}
// =======================
//   FileProcessor (варіант 1)
// =======================
public class FileProcessor
{
    private int attempts = 0;

    public string ReadFile(string path)
    {
        attempts++;

        if (attempts <= 2)
        {
            throw new FileNotFoundException("Файл не знайдено (імітація помилки).");
        }

        return "Файл успішно прочитано!";
    }
}
// =======================
//   NetworkClient (варіант 1)
// =======================
public class NetworkClient
{
    private int attempts = 0;

    public string DownloadData(string url)
    {
        attempts++;

        if (attempts <= 3)
        {
            throw new HttpRequestException("Мережева помилка (імітація).");
        }

        return "Дані успішно завантажені!";
    }
}