using mini_project_pharmacy.Domain.Exceptions;

namespace mini_project_pharmacy.Domain.Models;

public sealed class Order
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CustomerId { get; init; }
    public IReadOnlyList<OrderLine> Lines => _lines.AsReadOnly();
    public decimal TotalPrice => _lines.Sum(x => x.LineTotal);
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    private readonly List<OrderLine> _lines = new();

    public Order(Guid customerId)
    {
        CustomerId = customerId;
    }

    public void AddLine(Medicine medicine, int quantity, decimal pricePerUnit)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        var existing = _lines.FirstOrDefault(x => x.MedicineId == medicine.Id);
        if (existing != null)
        {
            existing.IncreaseQuantity(quantity);
        }
        else
        {
            _lines.Add(new OrderLine(medicine.Id, medicine.Name, quantity, pricePerUnit));
        }
    }
}

public sealed class OrderLine
{
    public Guid MedicineId { get; init; }
    public string MedicineName { get; init; }
    public int Quantity { get; private set; }
    public decimal PricePerUnit { get; init; }
    public decimal LineTotal => Quantity * PricePerUnit;

    public OrderLine(Guid medicineId, string medicineName, int quantity, decimal pricePerUnit)
    {
        MedicineId = medicineId;
        MedicineName = medicineName;
        Quantity = quantity;
        PricePerUnit = pricePerUnit;
    }

    public void IncreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new DomainException("Increase amount must be greater than zero.");
        Quantity += amount;
    }
}

