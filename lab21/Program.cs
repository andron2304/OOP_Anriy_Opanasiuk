using System;

namespace IndependentWork21
{
    // From Lab19: Factory and Singleton
    public interface ILogger
    {
        void Log(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[ConsoleLogger] {message}");
        }
    }

    public class FileLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[FileLogger] Записано у файл: {message}");
        }
    }

    public class DatabaseLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[DatabaseLogger] Записано у БД: {message}");
        }
    }

    public abstract class LoggerFactory
    {
        public abstract ILogger CreateLogger();
    }

    public class ConsoleLoggerFactory : LoggerFactory
    {
        public override ILogger CreateLogger() => new ConsoleLogger();
    }

    public class FileLoggerFactory : LoggerFactory
    {
        public override ILogger CreateLogger() => new FileLogger();
    }

    public class DatabaseLoggerFactory : LoggerFactory
    {
        public override ILogger CreateLogger() => new DatabaseLogger();
    }

    public class LoggerManager
    {
        private static readonly Lazy<LoggerManager> _instance = new Lazy<LoggerManager>(() => new LoggerManager());
        private ILogger _logger;

        private LoggerManager() { }

        public static LoggerManager Instance => _instance.Value;

        public void SetLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string message)
        {
            _logger?.Log(message);
        }
    }

    // From Lab20: Strategy and Observer
    public interface IDataProcessorStrategy
    {
        void Process(string data);
    }

    public class AddBookStrategy : IDataProcessorStrategy
    {
        public void Process(string data)
        {
            Console.WriteLine($"Додавання книги: {data}");
        }
    }

    public class RemoveBookStrategy : IDataProcessorStrategy
    {
        public void Process(string data)
        {
            Console.WriteLine($"Видалення книги: {data}");
        }
    }

    public class UpdateBookInfoStrategy : IDataProcessorStrategy
    {
        public void Process(string data)
        {
            Console.WriteLine($"Оновлення інформації про книгу: {data}");
        }
    }

    public class DataContext
    {
        private IDataProcessorStrategy _strategy;

        public DataContext(IDataProcessorStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public void SetStrategy(IDataProcessorStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public void ExecuteProcessing(string data)
        {
            Console.WriteLine($"[DataContext] Виконання стратегії: {data}");
            _strategy.Process(data);
        }
    }

    public class DataPublisher
    {
        public event Action<string>? DataProcessed;

        public void PublishDataProcessed(string data)
        {
            Console.WriteLine("[DataPublisher] Публікація події обробки даних...");
            DataProcessed?.Invoke(data);
        }
    }

    // Modified observers to use logger
    public class LibraryCatalogObserver
    {
        public void OnDataProcessed(string data)
        {
            LoggerManager.Instance.Log($"Оновлено каталог з: {data}");
        }
    }

    public class NewArrivalsNotifierObserver
    {
        public void OnDataProcessed(string data)
        {
            LoggerManager.Instance.Log($"Повідомлено про нову надходження: {data}");
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            // Setup logger via factory and singleton
            LoggerFactory factory = new ConsoleLoggerFactory();
            ILogger logger = factory.CreateLogger();
            LoggerManager.Instance.SetLogger(logger);

            // Setup data processing
            var dataContext = new DataContext(new AddBookStrategy());
            var publisher = new DataPublisher();

            var catalogObserver = new LibraryCatalogObserver();
            var notifierObserver = new NewArrivalsNotifierObserver();

            publisher.DataProcessed += catalogObserver.OnDataProcessed;
            publisher.DataProcessed += notifierObserver.OnDataProcessed;

            // Demonstrate integration
            Console.WriteLine("=== Інтегрована система: Демонстрація ===");
            dataContext.SetStrategy(new AddBookStrategy());
            dataContext.ExecuteProcessing("Книга: '1984' автора Джордж Орвелл");
            publisher.PublishDataProcessed("Подія додавання книги");

            dataContext.SetStrategy(new RemoveBookStrategy());
            dataContext.ExecuteProcessing("Книга: 'Убити пересмішника' автора Гарпер Лі");
            publisher.PublishDataProcessed("Подія видалення книги");
        }
    }
}