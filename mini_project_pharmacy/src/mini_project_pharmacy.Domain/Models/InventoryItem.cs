using mini_project_pharmacy.Domain.Exceptions;

namespace mini_project_pharmacy.Domain.Models;

public sealed class InventoryItem
{
    public Medicine Medicine { get; init; } = new Medicine();
    public int Quantity { get; set; }
    public int ReorderThreshold { get; init; }
    public int ReorderAmount { get; init; }

    public InventoryItem() { }

    public InventoryItem(Medicine medicine, int quantity, int reorderThreshold, int reorderAmount)
    {
        Medicine = medicine;
        Quantity = quantity;
        ReorderThreshold = reorderThreshold;
        ReorderAmount = reorderAmount;
    }

    public bool NeedsReorder => Quantity <= ReorderThreshold;
    public bool IsAvailable => Quantity > 0 && !Medicine.IsExpired(DateTime.UtcNow);

    public void Reserve(int amount)
    {
        if (amount <= 0)
            throw new DomainException("Reserved amount must be greater than zero.");
        if (amount > Quantity)
            throw new DomainException($"Not enough stock for {Medicine.Name}. Requested {amount}, available {Quantity}.");
        Quantity -= amount;
    }

    public void Restock(int amount)
    {
        if (amount <= 0)
            throw new DomainException("Restock amount must be greater than zero.");
        Quantity += amount;
    }
}

