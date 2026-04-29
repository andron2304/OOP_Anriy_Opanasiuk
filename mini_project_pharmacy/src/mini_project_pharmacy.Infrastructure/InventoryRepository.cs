using mini_project_pharmacy.Domain.Contracts;
using mini_project_pharmacy.Domain.Models;

namespace mini_project_pharmacy.Infrastructure;

public sealed class InventoryRepository : IInventoryRepository
{
    private readonly IPersistenceService _persistenceService;
    private readonly List<InventoryItem> _items;

    public InventoryRepository(IPersistenceService persistenceService)
    {
        _persistenceService = persistenceService;
        _items = _persistenceService.LoadInventory().ToList();
    }

    public IReadOnlyCollection<InventoryItem> GetAllItems() => _items.AsReadOnly();

    public InventoryItem? GetItem(Guid medicineId) => _items.FirstOrDefault(item => item.Medicine.Id == medicineId);

    public void AddOrUpdateItem(InventoryItem item)
    {
        var existing = _items.FirstOrDefault(x => x.Medicine.Id == item.Medicine.Id);
        if (existing is null)
            _items.Add(item);
        else
        {
            _items.Remove(existing);
            _items.Add(item);
        }
    }

    public IEnumerable<InventoryItem> GetLowStockItems() => _items.Where(item => item.NeedsReorder).ToList();

    public void Save() => _persistenceService.StoreInventory(_items);
}

