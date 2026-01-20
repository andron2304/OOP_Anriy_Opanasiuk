using System;

#region Модель

public class Report
{
    public string Title { get; }
    public string Content { get; }

    public Report(string title, string content)
    {
        Title = title;
        Content = content;
    }
}

#endregion

#region SRP: приклад порушення

// Порушує SRP: відповідає і за форматування, і за збереження, і за вивід
public class BadReportManager
{
    public void GenerateAndSaveReport(Report report)
    {
        string formatted = $"*** {report.Title} ***\n{report.Content}";
        Console.WriteLine(formatted);
        Console.WriteLine("Звіт збережено у файл");
    }
}

#endregion

#region SRP: правильна декомпозиція

public interface IReportFormatter
{
    string Format(Report report);
}

public interface IReportSaver
{
    void Save(string formattedReport);
}

public class SimpleReportFormatter : IReportFormatter
{
    public string Format(Report report)
    {
        return $"*** {report.Title} ***\n{report.Content}";
    }
}

public class ConsoleReportSaver : IReportSaver
{
    public void Save(string formattedReport)
    {
        Console.WriteLine(formattedReport);
        Console.WriteLine("Звіт збережено (консоль)");
    }
}

#endregion

#region OCP: приклад дотримання

public interface IDiscountStrategy
{
    decimal Apply(decimal amount);
}

public class NoDiscount : IDiscountStrategy
{
    public decimal Apply(decimal amount) => amount;
}

public class PercentageDiscount : IDiscountStrategy
{
    private readonly decimal _percent;

    public PercentageDiscount(decimal percent)
    {
        _percent = percent;
    }

    public decimal Apply(decimal amount)
    {
        return amount - amount * _percent;
    }
}

public class PriceCalculator
{
    private readonly IDiscountStrategy _discountStrategy;

    public PriceCalculator(IDiscountStrategy discountStrategy)
    {
        _discountStrategy = discountStrategy;
    }

    public decimal Calculate(decimal amount)
    {
        return _discountStrategy.Apply(amount);
    }
}

#endregion

class Program
{
    static void Main()
    {
        // SRP (правильний варіант)
        var report = new Report("Звіт", "Аналіз принципів SOLID");
        IReportFormatter formatter = new SimpleReportFormatter();
        IReportSaver saver = new ConsoleReportSaver();

        string formatted = formatter.Format(report);
        saver.Save(formatted);

        // OCP
        PriceCalculator calculator = new PriceCalculator(new PercentageDiscount(0.1m));
        Console.WriteLine("Ціна зі знижкою: " + calculator.Calculate(100));
    }
}