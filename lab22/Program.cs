using System;
using System.Collections.Generic;

namespace IndependentWork22
{
    public interface IComponent
    {
        double GetValue();
    }

    public class LengthUnit : IComponent
    {
        public double Value { get; }
        public string Unit { get; }

        public LengthUnit(double value, string unit)
        {
            Value = value;
            Unit = unit;
        }

        public double GetValue()
        {
            Console.WriteLine($"Одиниця довжини: {Value} {Unit}");
            return Value;
        }
    }

    public class WeightUnit : IComponent
    {
        public double Value { get; }
        public string Unit { get; }

        public WeightUnit(double value, string unit)
        {
            Value = value;
            Unit = unit;
        }

        public double GetValue()
        {
            Console.WriteLine($"Одиниця ваги: {Value} {Unit}");
            return Value;
        }
    }

    public class MeasurementGroup : IComponent
    {
        private List<IComponent> _components = new List<IComponent>();

        public void Add(IComponent component)
        {
            _components.Add(component);
        }

        public void Remove(IComponent component)
        {
            _components.Remove(component);
        }

        public double GetValue()
        {
            double total = 0;
            Console.WriteLine("Група вимірювань:");
            foreach (var component in _components)
            {
                total += component.GetValue();
            }
            Console.WriteLine($"Загальне значення: {total}");
            return total;
        }
    }

    public abstract class Decorator : IComponent
    {
        protected IComponent _component;

        public Decorator(IComponent component)
        {
            _component = component;
        }

        public virtual double GetValue()
        {
            return _component.GetValue();
        }
    }

    public class PrecisionDecorator : Decorator
    {
        private int _decimals;

        public PrecisionDecorator(IComponent component, int decimals) : base(component)
        {
            _decimals = decimals;
        }

        public override double GetValue()
        {
            double value = _component.GetValue();
            double rounded = Math.Round(value, _decimals);
            Console.WriteLine($"Округлено до {_decimals} знаків: {rounded}");
            return rounded;
        }
    }

    public class UnitConverterDecorator : Decorator
    {
        private string _targetUnit;

        public UnitConverterDecorator(IComponent component, string targetUnit) : base(component)
        {
            _targetUnit = targetUnit;
        }

        public override double GetValue()
        {
            double value = _component.GetValue();
            // Імітація конвертації: наприклад, м до см (помножити на 100)
            double converted = value * 100; // Простий приклад
            Console.WriteLine($"Конвертовано до {_targetUnit}: {converted}");
            return converted;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            // Створення окремих одиниць
            var length = new LengthUnit(5.123, "м");
            var weight = new WeightUnit(10.456, "кг");

            // Створення групи
            var group = new MeasurementGroup();
            group.Add(length);
            group.Add(weight);

            // Декорування
            var preciseLength = new PrecisionDecorator(length, 1);
            var convertedLength = new UnitConverterDecorator(length, "см");

            var preciseGroup = new PrecisionDecorator(group, 2);

            Console.WriteLine("=== Окремі одиниці ===");
            length.GetValue();
            weight.GetValue();

            Console.WriteLine("\n=== Група ===");
            group.GetValue();

            Console.WriteLine("\n=== Декоровані одиниці ===");
            preciseLength.GetValue();
            convertedLength.GetValue();

            Console.WriteLine("\n=== Декорована група ===");
            preciseGroup.GetValue();
        }
    }
}