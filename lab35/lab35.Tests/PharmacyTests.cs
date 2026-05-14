using Xunit;
using lab35.Domain;
using lab35.Services;

namespace lab35.Tests;

public class PharmacyTests
{
    [Fact]
    public void PensionerDiscount_ShouldApply15Percent()
    {
        var strategy = new PensionerDiscount();
        var result = strategy.Apply(100m);
        Assert.Equal(85m, result);
    }

    [Fact]
    public void Medication_GetFinalPrice_ThrowsOnZeroPrice()
    {
        var med = new Medication { BasePrice = 0 };
        Assert.Throws<ArgumentException>(() => med.GetFinalPrice(new NoDiscount()));
    }

    [Fact]
    public void Service_TotalValue_CalculatesCorrectSum()
    {
        var service = new PharmacyService();
        var list = new List<Medication> {
            new Medication { BasePrice = 100 },
            new Medication { BasePrice = 200 }
        };
        service.Refresh(list);
        Assert.Equal(300m, service.GetTotalInventoryValue());
    }
    
    // Додай ще подібні тести для LINQ фільтрів, щоб було 12
}