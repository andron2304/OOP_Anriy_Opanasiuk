using mini_project_pharmacy.Domain.Contracts;
using mini_project_pharmacy.Domain.Exceptions;
using mini_project_pharmacy.Domain.Models;

namespace mini_project_pharmacy.Domain;

public sealed class Pharmacy
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IPricingStrategy _pricingStrategy;

    public Pharmacy(IInventoryRepository inventoryRepository, IPricingStrategy pricingStrategy)
    {
        _inventoryRepository = inventoryRepository;
        _pricingStrategy = pricingStrategy;
    }

    public IReadOnlyCollection<InventoryItem> Inventory => _inventoryRepository.GetAllItems();

    public OrderResult ProcessPrescriptionOrder(Customer customer, Prescription prescription, IReadOnlyCollection<(Guid medicineId, int quantity)> orderLines)
    {
        var now = DateTime.UtcNow;
        if (!prescription.IsValid(now))
            return OrderResult.Fail("Prescription has expired.");
        if (prescription.CustomerId != customer.Id)
            return OrderResult.Fail("Prescription does not belong to the customer.");

        var order = new Order(customer.Id);
        foreach (var line in orderLines)
        {
            var item = _inventoryRepository.GetItem(line.medicineId);
            if (item is null)
                return OrderResult.Fail($"Medicine {line.medicineId} not found in inventory.");
            if (!item.IsAvailable)
                return OrderResult.Fail($"Medicine {item.Medicine.Name} is not available or expired.");
            if (item.Medicine.IsPrescriptionOnly && prescription.MedicineId != item.Medicine.Id)
                return OrderResult.Fail($"Prescription does not cover medicine {item.Medicine.Name}.");
            if (line.quantity > item.Quantity)
                return OrderResult.Fail($"Insufficient stock for {item.Medicine.Name}.");
            item.Reserve(line.quantity);
            var price = _pricingStrategy.CalculatePrice(item.Medicine, line.quantity, customer.IsInsured);
            order.AddLine(item.Medicine, line.quantity, price);
            _inventoryRepository.AddOrUpdateItem(item);
        }

        _inventoryRepository.Save();
        return OrderResult.Success(order);
    }

    public InventoryReport GenerateInventoryReport()
    {
        var snapshot = Inventory.Select(item => new InventoryReportLine(item.Medicine.Name, item.Quantity, item.NeedsReorder ? "Reorder" : "OK", item.Medicine.IsExpired(DateTime.UtcNow))).ToList();
        return new InventoryReport(snapshot);
    }

    public void RestockMedicine(Guid medicineId, int quantity)
    {
        var item = _inventoryRepository.GetItem(medicineId) ?? throw new DomainException("Medicine not found.");
        item.Restock(quantity);
        _inventoryRepository.AddOrUpdateItem(item);
        _inventoryRepository.Save();
    }
}

public sealed class InventoryReport
{
    public IReadOnlyCollection<InventoryReportLine> Lines { get; }

    public InventoryReport(IReadOnlyCollection<InventoryReportLine> lines)
    {
        Lines = lines;
    }
}

public sealed class InventoryReportLine
{
    public string MedicineName { get; }
    public int Quantity { get; }
    public string Status { get; }
    public bool IsExpired { get; }

    public InventoryReportLine(string medicineName, int quantity, string status, bool isExpired)
    {
        MedicineName = medicineName;
        Quantity = quantity;
        Status = status;
        IsExpired = isExpired;
    }
}

public sealed class OrderResult
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public Order? Order { get; }

    private OrderResult(bool isSuccess, string message, Order? order)
    {
        IsSuccess = isSuccess;
        Message = message;
        Order = order;
    }

    public static OrderResult Success(Order order) => new(true, "Order processed successfully.", order);
    public static OrderResult Fail(string message) => new(false, message, null);
}

