using mini_project_pharmacy.Domain.Models;

namespace mini_project_pharmacy.Domain.Contracts;

public interface IInventoryRepository
{
    IReadOnlyCollection<InventoryItem> GetAllItems();
    InventoryItem? GetItem(Guid medicineId);
    void AddOrUpdateItem(InventoryItem item);
    void Save();
    IEnumerable<InventoryItem> GetLowStockItems();
}

