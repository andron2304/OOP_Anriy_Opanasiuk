using System;

namespace IndependentWork23
{
    // Adapter pattern
    public interface IPrintJob
    {
        void Print(string document);
    }

    public class LegacyPrinterDriver
    {
        public void SendToPrinter(byte[] rawData)
        {
            string text = System.Text.Encoding.UTF8.GetString(rawData);
            Console.WriteLine($"Драйвер старого принтера: друк документа -> {text}");
        }
    }

    public class PrinterDriverAdapter : IPrintJob
    {
        private readonly LegacyPrinterDriver _legacyPrinter;

        public PrinterDriverAdapter(LegacyPrinterDriver legacyPrinter)
        {
            _legacyPrinter = legacyPrinter;
        }

        public void Print(string document)
        {
            byte[] rawData = System.Text.Encoding.UTF8.GetBytes(document);
            Console.WriteLine("Адаптер драйвера принтера: адаптація документа для старого драйвера...");
            _legacyPrinter.SendToPrinter(rawData);
        }
    }

    // Facade pattern
    public class DocumentFormatter
    {
        public string FormatDocument(string title, string body)
        {
            return $"=== {title} ===\n{body}\n";
        }
    }

    public class PrintQueue
    {
        public void AddJob(string formattedDocument)
        {
            Console.WriteLine($"Черга друку: додано до черги друку:\n{formattedDocument}");
        }
    }

    public class PrinterFacade
    {
        private readonly DocumentFormatter _formatter;
        private readonly PrintQueue _queue;
        private readonly IPrintJob _printer;

        public PrinterFacade(DocumentFormatter formatter, PrintQueue queue, IPrintJob printer)
        {
            _formatter = formatter;
            _queue = queue;
            _printer = printer;
        }

        public void PrintDocument(string title, string body)
        {
            string formatted = _formatter.FormatDocument(title, body);
            _queue.AddJob(formatted);
            Console.WriteLine("Фасад принтера: передача документа на друк...");
            _printer.Print(formatted);
        }
    }

    // Proxy pattern
    public interface IPrinter
    {
        void Print(string document);
    }

    public class RealPrinter : IPrinter
    {
        public void Print(string document)
        {
            Console.WriteLine($"RealPrinter: друк документа на фізичному принтері...\n{document}");
        }
    }

    public class SecurityPrinterProxy : IPrinter
    {
        private readonly IPrinter _realPrinter;
        private readonly string _userRole;

        public SecurityPrinterProxy(IPrinter realPrinter, string userRole)
        {
            _realPrinter = realPrinter;
            _userRole = userRole;
        }

        public void Print(string document)
        {
            if (_userRole != "адмін")
            {
                Console.WriteLine("Проксі безпеки принтера: доступ заборонено. Потрібна роль 'адмін'.");
                return;
            }

            Console.WriteLine("Проксі безпеки принтера: доступ дозволено, друк через реальний принтер...");
            _realPrinter.Print(document);
        }
    }

    public class CachingDataLoaderProxy : IPrinter
    {
        private readonly IPrinter _realPrinter;
        private string? _cachedDocument;

        public CachingDataLoaderProxy(IPrinter realPrinter)
        {
            _realPrinter = realPrinter;
        }

        public void Print(string document)
        {
            if (_cachedDocument == document)
            {
                Console.WriteLine("Проксі кешування: використовується кешований документ, друк повторно не виконано.");
                return;
            }

            Console.WriteLine("Проксі кешування: кешування документа перед друком...");
            _cachedDocument = document;
            _realPrinter.Print(document);
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("=== Адаптер ===");
            var legacyPrinter = new LegacyPrinterDriver();
            IPrintJob adapter = new PrinterDriverAdapter(legacyPrinter);
            adapter.Print("Документ для друку: Варіант 13");

            Console.WriteLine("\n=== Фасад ===");
            var formatter = new DocumentFormatter();
            var queue = new PrintQueue();
            var facadePrinter = new PrinterDriverAdapter(legacyPrinter);
            var printerFacade = new PrinterFacade(formatter, queue, facadePrinter);
            printerFacade.PrintDocument("Звіт", "Це тестовий документ для друку через фасад.");

            Console.WriteLine("\n=== Проксі ===");
            IPrinter realPrinter = new RealPrinter();
            IPrinter securityProxy = new SecurityPrinterProxy(realPrinter, "користувач");
            securityProxy.Print("Секретний документ що не можна друкувати");

            Console.WriteLine();
            IPrinter adminProxy = new SecurityPrinterProxy(realPrinter, "адмін");
            IPrinter cachingProxy = new CachingDataLoaderProxy(adminProxy);
            cachingProxy.Print("Документ для друку з кешуванням");
            cachingProxy.Print("Документ для друку з кешуванням");
        }
    }
}
