using Xunit;
using lab30vN;
using System;

public class CurrencyConverterTests
{
    CurrencyConverter converter = new CurrencyConverter();

    [Fact]
    public void Test1()
    {
        Assert.Equal(1, converter.GetRate("USD"));
    }

    [Fact]
    public void Test2()
    {
        Assert.Equal(0.9m, converter.GetRate("EUR"));
    }

    [Fact]
    public void Test3()
    {
        Assert.Throws<ArgumentException>(() => converter.GetRate("ABC"));
    }

    [Fact]
    public void Test4()
    {
        Assert.Equal(9, converter.Convert("USD","EUR",10));
    }

    [Fact]
    public void Test5()
    {
        Assert.Equal(10, converter.Convert("EUR","USD",9));
    }

    [Fact]
    public void Test6()
    {
        Assert.Equal(1, converter.Convert("UAH","USD",40));
    }

    [Fact]
    public void Test7()
    {
        Assert.Throws<ArgumentException>(() => converter.Convert("USD","EUR",-5));
    }

    [Theory]
    [InlineData(10,9)]
    [InlineData(20,18)]
    [InlineData(100,90)]
    public void Test8(decimal usd, decimal expected)
    {
        Assert.Equal(expected, converter.Convert("USD","EUR",usd));
    }

    [Theory]
    [InlineData(40,1)]
    [InlineData(80,2)]
    public void Test9(decimal uah, decimal expected)
    {
        Assert.Equal(expected, converter.Convert("UAH","USD",uah));
    }
}