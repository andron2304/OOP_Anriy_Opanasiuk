using System;

namespace IndependentWork20
{
    public interface IDataProcessorStrategy
    {
        void Process(string data);
    }

    public class AddBookStrategy : IDataProcessorStrategy
    {
        public void Process(string data)
        {
            Console.WriteLine($"Adding book: {data}");
        }
    }

    public class RemoveBookStrategy : IDataProcessorStrategy
    {
        public void Process(string data)
        {
            Console.WriteLine($"Removing book: {data}");
        }
    }

    public class UpdateBookInfoStrategy : IDataProcessorStrategy
    {
        public void Process(string data)
        {
            Console.WriteLine($"Updating book info: {data}");
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
            Console.WriteLine($"[DataContext] Executing strategy: {data}");
            _strategy.Process(data);
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

    public class LibraryCatalogObserver
    {
        public void OnDataProcessed(string data)
        {
            Console.WriteLine($"[LibraryCatalogObserver] Updated catalog with: {data}");
        }
    }

    public class NewArrivalsNotifierObserver
    {
        public void OnDataProcessed(string data)
        {
            Console.WriteLine($"[NewArrivalsNotifierObserver] Notified about new arrival: {data}");
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var dataContext = new DataContext(new AddBookStrategy());
            var publisher = new DataPublisher();

            var catalogObserver = new LibraryCatalogObserver();
            var notifierObserver = new NewArrivalsNotifierObserver();

            publisher.DataProcessed += catalogObserver.OnDataProcessed;
            publisher.DataProcessed += notifierObserver.OnDataProcessed;

            Console.WriteLine("=== Strategy: AddBookStrategy ===");
            dataContext.SetStrategy(new AddBookStrategy());
            dataContext.ExecuteProcessing("Book: '1984' by George Orwell");
            publisher.PublishDataProcessed("Book added event");

            Console.WriteLine("\n=== Strategy: RemoveBookStrategy ===");
            dataContext.SetStrategy(new RemoveBookStrategy());
            dataContext.ExecuteProcessing("Book: 'To Kill a Mockingbird' by Harper Lee");
            publisher.PublishDataProcessed("Book removed event");

            Console.WriteLine("\n=== Strategy: UpdateBookInfoStrategy ===");
            dataContext.SetStrategy(new UpdateBookInfoStrategy());
            dataContext.ExecuteProcessing("Book: 'Pride and Prejudice' by Jane Austen - Updated price");
            publisher.PublishDataProcessed("Book updated event");
        }
    }
}
