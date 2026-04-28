using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace IndependentWork24
{
    /// <summary>
    /// Інтерфейс компоненту для Composite паттерну.
    /// Дозволяє працювати з окремими товарами та їхніми наборами через єдиний інтерфейс.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Отримує вартість компоненту (товару або набору товарів).
        /// </summary>
        double GetValue();
    }

    /// <summary>
    /// Клас, що представляє окремий товар.
    /// Це листовий вузол в Composite структурі.
    /// </summary>
    public class Product : IComponent
    {
        /// <summary>Назва товару</summary>
        public string Name { get; }

        /// <summary>Ціна товару</summary>
        public double Price { get; }

        /// <summary>Конструктор товару з назвою та ціною.</summary>
        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }

        /// <summary>Повертає ціну товару та виводить інформацію в консоль.</summary>
        public double GetValue()
        {
            Console.WriteLine($"Product: {Name}, price = {Price}");
            return Price;
        }
    }

    /// <summary>
    /// Клас, що представляє набір товарів (контейнер).
    /// Це гілка в Composite структурі. Може містити як товари, так і інші набори.
    /// </summary>
    public class ProductBundle : IComponent
    {
        /// <summary>Список елементів у наборі</summary>
        private readonly List<IComponent> _items = new();

        /// <summary>Назва набору товарів</summary>
        public string Name { get; }

        /// <summary>Конструктор набору з назвою.</summary>
        public ProductBundle(string name)
        {
            Name = name;
        }

        /// <summary>Додає елемент (товар або набір) до цього набору.</summary>
        public void Add(IComponent item)
        {
            _items.Add(item);
        }

        /// <summary>Видаляє елемент із цього набору.</summary>
        public void Remove(IComponent item)
        {
            _items.Remove(item);
        }

        /// <summary>
        /// Обчислює загальну вартість всіх елементів у наборі.
        /// Рекурсивно викликає GetValue() для кожного елемента.
        /// </summary>
        public double GetValue()
        {
            Console.WriteLine($"Bundle: {Name}");
            double sum = 0;
            foreach (var item in _items)
            {
                sum += item.GetValue();
            }
            Console.WriteLine($"Bundle total = {sum}");
            return sum;
        }
    }

    /// <summary>
    /// Абстрактний базовий клас для всіх декораторів.
    /// Декоратор оборотує компонент і додає йому додаткову функціональність.
    /// </summary>
    public abstract class Decorator : IComponent
    {
        /// <summary>Внутрішній компонент, який обгортається декоратором</summary>
        protected IComponent Component { get; }

        /// <summary>Конструктор, що приймає компонент для обгортання.</summary>
        protected Decorator(IComponent component)
        {
            Component = component;
        }

        /// <summary>
        /// За умовчанням просто повертає вартість внутрішнього компоненту.
        /// Підкласи можуть перевизначити цей метод для додавання спеціальної логіки.
        /// </summary>
        public virtual double GetValue()
        {
            return Component.GetValue();
        }
    }

    /// <summary>
    /// Декоратор, що застосовує знижку до компоненту.
    /// Множить вартість на коефіцієнт (1 - процент / 100).
    /// </summary>
    public class DiscountDecorator : Decorator
    {
        /// <summary>Відсоток знижки</summary>
        private readonly double _percent;

        /// <summary>Конструктор з компонентом та відсотком знижки.</summary>
        public DiscountDecorator(IComponent component, double percent) : base(component)
        {
            _percent = percent;
        }

        /// <summary>
        /// Обчислює вартість із застосуванням знижки.
        /// Формула: базова_вартість * (1 - процент / 100)
        /// </summary>
        public override double GetValue()
        {
            double baseValue = Component.GetValue();
            double discounted = baseValue * (1 - _percent / 100.0);
            Console.WriteLine($"DiscountDecorator: {baseValue} -> {discounted} after {_percent}% discount");
            return discounted;
        }
    }

    /// <summary>
    /// Декоратор, що застосовує податок до компоненту.
    /// Множить вартість на коефіцієнт (1 + процент / 100).
    /// </summary>
    public class TaxDecorator : Decorator
    {
        /// <summary>Відсоток податку</summary>
        private readonly double _percent;

        /// <summary>Конструктор з компонентом та відсотком податку.</summary>
        public TaxDecorator(IComponent component, double percent) : base(component)
        {
            _percent = percent;
        }

        /// <summary>
        /// Обчислює вартість із застосуванням податку.
        /// Формула: базова_вартість * (1 + процент / 100)
        /// </summary>
        public override double GetValue()
        {
            double baseValue = Component.GetValue();
            double taxed = baseValue * (1 + _percent / 100.0);
            Console.WriteLine($"TaxDecorator: {baseValue} -> {taxed} after {_percent}% tax");
            return taxed;
        }
    }

    /// <summary>
    /// Інтерфейс для калькулятора ціни.
    /// Дозволяє різні реалізації: проста та з логуванням (Proxy).
    /// </summary>
    public interface IPriceCalculator
    {
        /// <summary>Обчислює вартість компоненту.</summary>
        double Calculate(IComponent component);
    }

    /// <summary>
    /// Простий калькулятор ціни без додаткової функціональності.
    /// Це справжня реалізація, яку обгортає Proxy.
    /// </summary>
    public class SimplePriceCalculator : IPriceCalculator
    {
        /// <summary>
        /// Обчислює вартість компоненту, викликаючи його метод GetValue().
        /// Викидає виключення, якщо компонент null.
        /// </summary>
        public double Calculate(IComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            return component.GetValue();
        }
    }

    /// <summary>
    /// Proxy калькулятора, що додає логування до операцій розрахунку.
    /// Контролює доступ до справжнього калькулятора та логує виклики.
    /// </summary>
    public class LoggingPriceCalculatorProxy : IPriceCalculator
    {
        /// <summary>Справжній калькулятор, якого обгортає цей Proxy</summary>
        private readonly IPriceCalculator _inner;

        /// <summary>Конструктор з внутрішнім калькулятором.</summary>
        public LoggingPriceCalculatorProxy(IPriceCalculator inner)
        {
            _inner = inner;
        }

        /// <summary>
        /// Обчислює вартість з додаванням логування.
        /// Логує початок і результат розрахунку.
        /// </summary>
        public double Calculate(IComponent component)
        {
            Console.WriteLine("LoggingPriceCalculatorProxy: початок розрахунку ціни...");
            double result = _inner.Calculate(component);
            Console.WriteLine($"LoggingPriceCalculatorProxy: результат = {result}");
            return result;
        }
    }

    /// <summary>
    /// Головна програма, що демонструє інтеграцію трьох структурних патернів:
    /// Composite, Decorator та Proxy.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Основна функція. Демонструє:
        /// 1. Composite: створення дерева товарів та наборів
        /// 2. Decorator: застосування знижок та податків
        /// 3. Proxy: логування операцій розрахунку
        /// 4. Порівняння продуктивності
        /// </summary>
        private static void Main(string[] args)
        {
            // Створюємо окремі товари
            var apple = new Product("Apple", 1.2);
            var orange = new Product("Orange", 0.8);
            var milk = new Product("Milk", 2.5);

            // Демонстрація Composite: створюємо ієрархію
            var fruitBundle = new ProductBundle("Fruit Bundle");
            fruitBundle.Add(apple);
            fruitBundle.Add(orange);

            var groceryBundle = new ProductBundle("Grocery Bundle");
            groceryBundle.Add(fruitBundle);
            groceryBundle.Add(milk);

            // Тестування Composite паттерну
            Console.WriteLine("=== Composite ===");
            groceryBundle.GetValue();

            // Демонстрація Decorator: послідовне застосування знижок та податків
            Console.WriteLine("\n=== Decorator ===");
            var discountedBundle = new DiscountDecorator(groceryBundle, 10);
            var taxedDiscountedBundle = new TaxDecorator(discountedBundle, 5);
            taxedDiscountedBundle.GetValue();

            // Демонстрація Proxy та порівняння продуктивності
            Console.WriteLine("\n=== Proxy + Performance comparison ===");
            var calculator = new SimplePriceCalculator();
            var proxyCalculator = new LoggingPriceCalculatorProxy(calculator);

            // Вимірюємо час без Proxy
            var stopwatch = Stopwatch.StartNew();
            double normalValue = calculator.Calculate(groceryBundle);
            stopwatch.Stop();
            Console.WriteLine($"Без проксі: {normalValue}, час = {stopwatch.ElapsedMilliseconds} ms");

            // Вимірюємо час з Proxy та Decorator
            stopwatch.Restart();
            double proxiedValue = proxyCalculator.Calculate(taxedDiscountedBundle);
            stopwatch.Stop();
            Console.WriteLine($"З проксі/декоратором: {proxiedValue}, час = {stopwatch.ElapsedMilliseconds} ms");

            // Висновки про патерни
            Console.WriteLine("\n=== Висновки ===");
            Console.WriteLine("1. Composite спрощує роботу з групою продуктів як з одним об'єктом.");
            Console.WriteLine("2. Decorator дозволяє додавати знижки та податки динамічно без зміни базових класів.");
            Console.WriteLine("3. Proxy додає логування без зміни калькулятора ціни, але додає накладні витрати на виконання.");
        }
    }
}
