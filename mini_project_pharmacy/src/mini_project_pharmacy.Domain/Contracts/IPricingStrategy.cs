using mini_project_pharmacy.Domain.Models;

namespace mini_project_pharmacy.Domain.Contracts;

public interface IPricingStrategy
{
    decimal CalculatePrice(Medicine medicine, int quantity, bool isInsured);
}

