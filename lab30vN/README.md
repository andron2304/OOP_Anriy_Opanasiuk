# Лабораторна робота №30

**Тема:** Написання юніт-тестів з xUnit
**Варіант 13:** CurrencyConverter (Convert, GetRate)
**Мета:** Навчитися писати юніт-тести для власного коду за допомогою xUnit, використовуючи різні типи assertion та параметризовані тести.

## 1. Створення проєктів

Відкрий термінал у папці репозиторію:

```bash
# Створюємо основний проект
dotnet new console -n lab30vN

# Створюємо тестовий проект
dotnet new xunit -n lab30vN.Tests

# Додаємо посилання на основний проект
dotnet add lab30vN.Tests reference lab30vN
```

Тепер структура папок буде приблизно така:

```
OOP_Anriy_Opanasiuk/
├─ lab30vN/
│  └─ lab30vN.csproj
├─ lab30vN.Tests/
│  └─ lab30vN.Tests.csproj
```

---

## 2. Реалізація класу CurrencyConverter

Файл: `lab30vN/CurrencyConverter.cs`

```csharp
using System;
using System.Collections.Generic;

namespace lab30vN
{
    public class CurrencyConverter
    {
        private Dictionary<string, decimal> rates = new Dictionary<string, decimal>()
        {
            {"USD", 1m},
            {"EUR", 0.9m},
            {"UAH", 40m}
        };

        public decimal GetRate(string currency)
        {
            if (!rates.ContainsKey(currency))
                throw new ArgumentException("Currency not supported");

            return rates[currency];
        }

        public decimal Convert(string from, string to, decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must be positive");

            decimal fromRate = GetRate(from);
            decimal toRate = GetRate(to);

            decimal usdAmount = amount / fromRate;
            return usdAmount * toRate;
        }
    }
}
```

Файл `Program.cs` можна залишити простим:

```csharp
using System;

namespace lab30vN
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Currency Converter");
        }
    }
}
```

---

## 3. Написання юніт-тестів

Файл: `lab30vN.Tests/UnitTest1.cs`

```csharp
using Xunit;
using lab30vN;
using System;

public class CurrencyConverterTests
{
    CurrencyConverter converter = new CurrencyConverter();

    [Fact]
    public void GetRate_USD_Returns1() => Assert.Equal(1m, converter.GetRate("USD"));

    [Fact]
    public void GetRate_EUR_Returns09() => Assert.Equal(0.9m, converter.GetRate("EUR"));

    [Fact]
    public void GetRate_InvalidCurrency_ThrowsException() =>
        Assert.Throws<ArgumentException>(() => converter.GetRate("BTC"));

    [Fact]
    public void Convert_USD_to_EUR() => Assert.Equal(9m, converter.Convert("USD", "EUR", 10));

    [Fact]
    public void Convert_EUR_to_USD() => Assert.Equal(10m, converter.Convert("EUR", "USD", 9));

    [Fact]
    public void Convert_UAH_to_USD() => Assert.Equal(1m, converter.Convert("UAH", "USD", 40));

    [Fact]
    public void Convert_NegativeAmount_ThrowsException() =>
        Assert.Throws<ArgumentException>(() => converter.Convert("USD", "EUR", -5));

    [Theory]
    [InlineData(10,9)]
    [InlineData(20,18)]
    [InlineData(100,90)]
    public void Convert_USD_to_EUR_Theory(decimal usd, decimal expected) =>
        Assert.Equal(expected, converter.Convert("USD","EUR",usd));

    [Theory]
    [InlineData(40,1)]
    [InlineData(80,2)]
    public void Convert_UAH_to_USD_Theory(decimal uah, decimal expected) =>
        Assert.Equal(expected, converter.Convert("UAH","USD",uah));
}
```

---

## 4. Запуск тестів

Перейдіть у каталог тестового проєкту:

```bash
cd lab30vN.Tests
dotnet test
```

**Результат:**

```
Passed! - Failed: 0
```

---

## 5. Що виконано

Створено два проєкти: `lab30vN` та `lab30vN.Tests`
Реалізовано клас `CurrencyConverter`
Методи: `GetRate()`, `Convert()`
Написано 10+ тестів
Використано `[Fact]` та `[Theory]`
Перевірка:
правильні обчислення
неправильна валюта
негативна сума
різні значення