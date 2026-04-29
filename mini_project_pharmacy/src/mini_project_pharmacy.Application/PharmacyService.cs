using mini_project_pharmacy.Domain;
using mini_project_pharmacy.Domain.Contracts;
using mini_project_pharmacy.Domain.Models;

namespace mini_project_pharmacy.Application;

public sealed class PharmacyService
{
    private readonly Pharmacy _pharmacy;

    public PharmacyService(IInventoryRepository inventoryRepository, IPricingStrategy pricingStrategy)
    {
        _pharmacy = new Pharmacy(inventoryRepository, pricingStrategy);
    }

    public PharmacySummary GetPharmacySummary()
    {
        var inventory = _pharmacy.Inventory;
        var lowStockCount = inventory.Count(item => item.NeedsReorder);
        return new PharmacySummary(inventory.Count, lowStockCount, DateTime.UtcNow);
    }

    public OrderResult CreatePrescriptionOrder(Customer customer, Prescription prescription, IReadOnlyCollection<(Guid medicineId, int quantity)> lines)
    {
        return _pharmacy.ProcessPrescriptionOrder(customer, prescription, lines);
    }

    public InventoryReport GetInventoryReport() => _pharmacy.GenerateInventoryReport();

    public void RestockMedicine(Guid medicineId, int quantity) => _pharmacy.RestockMedicine(medicineId, quantity);
}

public sealed class PharmacySummary
{
    public int TotalItems { get; }
    public int LowStockItems { get; }
    public DateTime GeneratedAt { get; }

    public PharmacySummary(int totalItems, int lowStockItems, DateTime generatedAt)
    {
        TotalItems = totalItems;
        LowStockItems = lowStockItems;
        GeneratedAt = generatedAt;
    }
}

