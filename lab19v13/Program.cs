using System;

namespace IndependentWork19
{
    public interface ILogger
    {
        void Log(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class FileLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[FILE] {message}");
        }
    }

    public class DatabaseLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[DB] {message}");
        }
    }

    public abstract class LoggerFactory
    {
        protected abstract ILogger CreateLogger();

        public void LogMessage(string message)
        {
            var logger = CreateLogger();
            logger.Log(message);
        }
    }

    public class ConsoleLoggerFactory : LoggerFactory
    {
        protected override ILogger CreateLogger()
        {
            return new ConsoleLogger();
        }
    }

    public class FileLoggerFactory : LoggerFactory
    {
        protected override ILogger CreateLogger()
        {
            return new FileLogger();
        }
    }

    public class DatabaseLoggerFactory : LoggerFactory
    {
        protected override ILogger CreateLogger()
        {
            return new DatabaseLogger();
        }
    }

    public sealed class LoggerManager
    {
        private static readonly Lazy<LoggerManager> _instance = new(() => new LoggerManager());
        private LoggerFactory _currentFactory;

        private LoggerManager()
        {
            _currentFactory = new ConsoleLoggerFactory();
        }

        public static LoggerManager Instance => _instance.Value;

        public void SetLoggerFactory(LoggerFactory factory)
        {
            _currentFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public void Log(string message)
        {
            if (_currentFactory == null)
            {
                throw new InvalidOperationException("Logger factory is not set.");
            }

            _currentFactory.LogMessage(message);
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var manager = LoggerManager.Instance;

            Console.WriteLine("=== Console Logger ===");
            manager.SetLoggerFactory(new ConsoleLoggerFactory());
            manager.Log("Початок роботи системи");
            manager.Log("Обробка події користувача");

            Console.WriteLine("\n=== File Logger ===");
            manager.SetLoggerFactory(new FileLoggerFactory());
            manager.Log("Запис повідомлення у файл");
            manager.Log("Файл збережено успішно");

            Console.WriteLine("\n=== Database Logger ===");
            manager.SetLoggerFactory(new DatabaseLoggerFactory());
            manager.Log("Початок транзакції в базі");
            manager.Log("Транзакція завершена");
        }
    }
}

