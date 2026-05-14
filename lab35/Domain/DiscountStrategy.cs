namespace lab35.Domain;

// Інтерфейс для стратегії знижок
public interface IDiscountStrategy
{
    decimal Apply(decimal price);
}

// Знижка для пенсіонерів (15%)
public class PensionerDiscount : IDiscountStrategy
{
    public decimal Apply(decimal price) => price * 0.85m;
}

// Без знижки
public class NoDiscount : IDiscountStrategy
{
    public decimal Apply(decimal price) => price;
}