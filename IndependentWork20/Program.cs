using System;

namespace IndependentWork20
{
    public interface IDataProcessorStrategy
    {
        string Process(string data);
    }

    public class EncryptDataStrategy : IDataProcessorStrategy
    {
        public string Process(string data)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));
        }
    }

    public class CompressDataStrategy : IDataProcessorStrategy
    {
        public string Process(string data)
        {
            return string.Join("", data.Split(' '));
        }
    }

    public class LogDataStrategy : IDataProcessorStrategy
    {
        public string Process(string data)
        {
            return data;
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

        public string ExecuteProcessing(string data)
        {
            Console.WriteLine($"[DataContext] Executing strategy: {data}");
            string result = _strategy.Process(data);
            Console.WriteLine($"Processed data: {result}");
            return result;
        }
    }

    public class DataPublisher
    {
        public event Action<string>? DataProcessed;

        public void PublishDataProcessed(string data)
        {
            Console.WriteLine("[DataPublisher] Publishing data processed event...");
            DataProcessed?.Invoke(data);
        }
    }

    public class ConsoleLoggerObserver
    {
        public void OnDataProcessed(string data)
        {
            Console.WriteLine($"[ConsoleLoggerObserver] Received processed data: {data}");
        }
    }

    public class FileSaverObserver
    {
        public void OnDataProcessed(string data)
        {
            Console.WriteLine($"[FileSaverObserver] Saved processed data to file: {data}");
        }
    }

    public class AnalyticsSenderObserver
    {
        public void OnDataProcessed(string data)
        {
            Console.WriteLine($"[AnalyticsSenderObserver] Sent analytics event for data: {data}");
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var dataContext = new DataContext(new EncryptDataStrategy());
            var publisher = new DataPublisher();

            var consoleObserver = new ConsoleLoggerObserver();
            var fileObserver = new FileSaverObserver();
            var analyticsObserver = new AnalyticsSenderObserver();

            publisher.DataProcessed += consoleObserver.OnDataProcessed;
            publisher.DataProcessed += fileObserver.OnDataProcessed;
            publisher.DataProcessed += analyticsObserver.OnDataProcessed;

            Console.WriteLine("=== Strategy: EncryptDataStrategy ===");
            dataContext.SetStrategy(new EncryptDataStrategy());
            string encryptedResult = dataContext.ExecuteProcessing("Hello World");
            publisher.PublishDataProcessed($"Encrypted: {encryptedResult}");

            Console.WriteLine("\n=== Strategy: CompressDataStrategy ===");
            dataContext.SetStrategy(new CompressDataStrategy());
            string compressedResult = dataContext.ExecuteProcessing("Hello World");
            publisher.PublishDataProcessed($"Compressed: {compressedResult}");

            Console.WriteLine("\n=== Strategy: LogDataStrategy ===");
            dataContext.SetStrategy(new LogDataStrategy());
            string loggedResult = dataContext.ExecuteProcessing("Hello World");
            publisher.PublishDataProcessed($"Logged: {loggedResult}");
        }
    }
}
