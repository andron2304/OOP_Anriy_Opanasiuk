using System;
using System.Collections.Generic;

#region Модель замовлення

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Order(int id, string customerName, decimal totalAmount)
    {
        Id = id;
        CustomerName = customerName;
        TotalAmount = totalAmount;
        Status = OrderStatus.New;
    }
}

public enum OrderStatus
{
    New,
    PendingValidation,
    Processed,
    Shipped,
    Delivered,
    Cancelled
}

#endregion

#region Початковий клас OrderProcessor (ПОРУШУЄ SRP)

public class OrderProcessor
{
    public void ProcessOrder(Order order)
    {
        Console.WriteLine("Початок обробки замовлення...");

        // Валідація
        if (order.TotalAmount <= 0)
        {
            Console.WriteLine("Помилка валідації: сума замовлення повинна бути більшою за 0");
            return;
        }

        // Збереження в "базу даних"
        Console.WriteLine($"Замовлення {order.Id} збережено в базі даних");

        // Надсилання email
        Console.WriteLine($"Email-повідомлення надіслано клієнту {order.CustomerName}");

        // Оновлення статусу
        order.Status = OrderStatus.Processed;
        Console.WriteLine($"Статус замовлення змінено на {order.Status}");
    }
}

#endregion

#region Інтерфейси (SRP)

public interface IOrderValidator
{
    bool IsValid(Order order);
}

public interface IOrderRepository
{
    void Save(Order order);
    Order GetById(int id);
}

public interface IEmailService
{
    void SendOrderConfirmation(Order order);
}

#endregion

#region Реалізації інтерфейсів (заглушки)

public class OrderValidator : IOrderValidator
{
    public bool IsValid(Order order)
    {
        return order.TotalAmount > 0;
    }
}

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly Dictionary<int, Order> _orders = new();

    public void Save(Order order)
    {
        _orders[order.Id] = order;
        Console.WriteLine($"Замовлення {order.Id} збережено в памʼяті");
    }

    public Order GetById(int id)
    {
        return _orders.ContainsKey(id) ? _orders[id] : null;
    }
}

public class ConsoleEmailService : IEmailService
{
    public void SendOrderConfirmation(Order order)
    {
        Console.WriteLine($"Підтвердження замовлення надіслано клієнту {order.CustomerName}");
    }
}

#endregion

#region OrderService (дотримання SRP + Dependency Injection)

public class OrderService
{
    private readonly IOrderValidator _validator;
    private readonly IOrderRepository _repository;
    private readonly IEmailService _emailService;

    public OrderService(
        IOrderValidator validator,
        IOrderRepository repository,
        IEmailService emailService)
    {
        _validator = validator;
        _repository = repository;
        _emailService = emailService;
    }

    public void ProcessOrder(Order order)
    {
        Console.WriteLine($"\nОбробка замовлення №{order.Id}");
        order.Status = OrderStatus.PendingValidation;

        if (!_validator.IsValid(order))
        {
            Console.WriteLine("Замовлення не пройшло валідацію!");
            order.Status = OrderStatus.Cancelled;
            return;
        }

        _repository.Save(order);
        _emailService.SendOrderConfirmation(order);

        order.Status = OrderStatus.Processed;
        Console.WriteLine($"Замовлення №{order.Id} успішно оброблено");
    }
}

#endregion

#region Метод Main (демонстрація)

class Program
{
    static void Main()
    {
        Console.WriteLine("=== OrderProcessor (порушення SRP) ===");
        var oldProcessor = new OrderProcessor();
        oldProcessor.ProcessOrder(new Order(1, "Іван", 100));

        Console.WriteLine("\n=== Рефакторинг із застосуванням SRP ===");

        IOrderValidator validator = new OrderValidator();
        IOrderRepository repository = new InMemoryOrderRepository();
        IEmailService emailService = new ConsoleEmailService();

        var orderService = new OrderService(validator, repository, emailService);

        // Валідне замовлення
        var validOrder = new Order(2, "Анна", 250);
        orderService.ProcessOrder(validOrder);

        // Невалідне замовлення
        var invalidOrder = new Order(3, "Петро", -50);
        orderService.ProcessOrder(invalidOrder);

        Console.WriteLine("\nНатисніть будь-яку клавішу для виходу...");
        Console.ReadKey();
    }
}

#endregion
