namespace App.Domain;

// Клас винятку має бути ТУТ і бути public
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

public class Order
{
    public Guid Id { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "New"; // Ініціалізація за замовчуванням

    // Конструктор для JSON
    public Order() 
    { 
        Status = string.Empty; 
    }

    public Order(Guid id, decimal totalAmount)
    {
        if (totalAmount < 0) throw new DomainException("Сума не може бути від'ємною.");
        Id = id;
        TotalAmount = totalAmount;
        Status = "New";
    }

    public void Pay()
    {
        if (Status != "New") throw new DomainException("Оплатити можна лише нове замовлення.");
        Status = "Paid";
    }
}