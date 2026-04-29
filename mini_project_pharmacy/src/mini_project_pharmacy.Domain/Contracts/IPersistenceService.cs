using mini_project_pharmacy.Domain.Models;

namespace mini_project_pharmacy.Domain.Contracts;

public interface IPersistenceService
{
    IReadOnlyCollection<InventoryItem> LoadInventory();
    void StoreInventory(IEnumerable<InventoryItem> items);
}

