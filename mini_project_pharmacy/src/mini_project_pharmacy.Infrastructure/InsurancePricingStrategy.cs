using mini_project_pharmacy.Domain.Contracts;
using mini_project_pharmacy.Domain.Models;

namespace mini_project_pharmacy.Infrastructure;

public sealed class InsurancePricingStrategy : IPricingStrategy
{
    private readonly decimal _insuranceDiscountRate;

    public InsurancePricingStrategy(decimal insuranceDiscountRate)
    {
        _insuranceDiscountRate = insuranceDiscountRate;
    }

    public decimal CalculatePrice(Medicine medicine, int quantity, bool isInsured)
    {
        var basePrice = medicine.Price * quantity;
        if (!isInsured)
            return basePrice;

        var discount = basePrice * _insuranceDiscountRate;
        return Math.Max(0m, basePrice - discount);
    }
}

