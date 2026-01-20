using System;
using System.Collections.Generic;

#region Strategy Interface

public interface IShippingStrategy
{
    decimal CalculateCost(decimal distance, decimal weight);
}

#endregion

#region Strategy Implementations

public class StandardShippingStrategy : IShippingStrategy
{
    public decimal CalculateCost(decimal distance, decimal weight)
    {
        return distance * 1.5m + weight * 0.5m;
    }
}

public class ExpressShippingStrategy : IShippingStrategy
{
    public decimal CalculateCost(decimal distance, decimal weight)
    {
        return (distance * 2.5m + weight * 1.0m) + 50m;
    }
}

public class InternationalShippingStrategy : IShippingStrategy
{
    public decimal CalculateCost(decimal distance, decimal weight)
    {
        decimal baseCost = distance * 5.0m + weight * 2.0m;
        return baseCost + baseCost * 0.15m;
    }
}

// Додаткова стратегія (демонстрація OCP)
public class NightShippingStrategy : IShippingStrategy
{
    public decimal CalculateCost(decimal distance, decimal weight)
    {
        decimal baseCost = distance * 1.5m + weight * 0.5m;
        return baseCost + 30m;
    }
}

#endregion

#region Factory Method

public static class ShippingStrategyFactory
{
    private static readonly Dictionary<string, IShippingStrategy> _strategies =
        new Dictionary<string, IShippingStrategy>(StringComparer.OrdinalIgnoreCase)
        {
            { "Standard", new StandardShippingStrategy() },
            { "Express", new ExpressShippingStrategy() },
            { "International", new InternationalShippingStrategy() },
            { "Night", new NightShippingStrategy() }
        };

    public static IShippingStrategy CreateStrategy(string deliveryType)
    {
        if (_strategies.ContainsKey(deliveryType))
        {
            return _strategies[deliveryType];
        }

        throw new ArgumentException("Невідомий тип доставки");
    }
}

#endregion

#region Context (Service)

public class DeliveryService
{
    public decimal CalculateDeliveryCost(
        decimal distance,
        decimal weight,
        IShippingStrategy strategy)
    {
        return strategy.CalculateCost(distance, weight);
    }
}

#endregion

class Program
{
    static void Main()
    {
        Console.WriteLine("Оберіть тип доставки:");
        Console.WriteLine("Standard, Express, International, Night");
        string type = Console.ReadLine();

        Console.Write("Введіть відстань (км): ");
        decimal distance = decimal.Parse(Console.ReadLine());

        Console.Write("Введіть вагу (кг): ");
        decimal weight = decimal.Parse(Console.ReadLine());

        try
        {
            IShippingStrategy strategy =
                ShippingStrategyFactory.CreateStrategy(type);

            DeliveryService service = new DeliveryService();
            decimal cost = service.CalculateDeliveryCost(distance, weight, strategy);

            Console.WriteLine($"Вартість доставки: {cost} грн");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
        Console.ReadKey();
    }
}

