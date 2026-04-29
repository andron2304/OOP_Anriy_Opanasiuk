using mini_project_pharmacy.Application;
using mini_project_pharmacy.Domain.Models;
using mini_project_pharmacy.Infrastructure;
using Xunit;

namespace mini_project_pharmacy.Tests;

public class PharmacyServiceTests
{
    [Fact]
    public void InsurancePricingStrategy_AppliesDiscountForInsuredCustomer()
    {
        var pricingStrategy = new InsurancePricingStrategy(0.2m);
        var medicine = new Medicine("Pain Reliever", "250mg", "Analgesic", 50m, false, DateTime.UtcNow.AddDays(100));
        var result = pricingStrategy.CalculatePrice(medicine, 2, isInsured: true);
        Assert.Equal(80m, result);
    }

    [Fact]
    public void Order_AddLine_MergesSameMedicineLines()
    {
        var order = new Order(Guid.NewGuid());
        var medicine = new Medicine("Vitamin C", "500mg", "Supplement", 10m, false, DateTime.UtcNow.AddDays(200));
        order.AddLine(medicine, 2, 10m);
        order.AddLine(medicine, 3, 10m);

        Assert.Single(order.Lines);
        Assert.Equal(5, order.Lines.First().Quantity);
        Assert.Equal(50m, order.TotalPrice);
    }
}

