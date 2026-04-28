using System;
using IndependentWork24;
using Xunit;

namespace tests;

public class IntegrationTests
{
    [Fact]
    public void Composite_CalculatesTotalValue()
    {
        var apple = new Product("Apple", 1.0);
        var orange = new Product("Orange", 2.0);
        var bundle = new ProductBundle("Fruit");
        bundle.Add(apple);
        bundle.Add(orange);

        double total = bundle.GetValue();

        Assert.Equal(3.0, total);
    }

    [Fact]
    public void Decorator_AppliesDiscountAndTax()
    {
        var product = new Product("Milk", 10.0);
        var discounted = new DiscountDecorator(product, 10);
        var taxed = new TaxDecorator(discounted, 20);

        double finalValue = taxed.GetValue();

        Assert.Equal(10.8, finalValue, 1);
    }

    [Fact]
    public void Proxy_LogsCalculationWithoutThrowing()
    {
        var bundle = new ProductBundle("Test");
        var calculator = new SimplePriceCalculator();
        var proxy = new LoggingPriceCalculatorProxy(calculator);

        double value = proxy.Calculate(bundle);

        Assert.Equal(0.0, value);
    }

    [Fact]
    public void Negative_NullComponent_ThrowsArgumentNullException()
    {
        var calculator = new SimplePriceCalculator();

        Assert.Throws<ArgumentNullException>(() => calculator.Calculate(null!));
    }
}
