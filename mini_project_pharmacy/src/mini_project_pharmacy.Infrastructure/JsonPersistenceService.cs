using System.Text.Json;
using mini_project_pharmacy.Domain.Contracts;
using mini_project_pharmacy.Domain.Models;

namespace mini_project_pharmacy.Infrastructure;

public sealed class JsonPersistenceService : IPersistenceService
{
    private readonly string _filePath;

    public JsonPersistenceService(string filePath)
    {
        _filePath = filePath;
    }

    public IReadOnlyCollection<InventoryItem> LoadInventory()
    {
        if (!File.Exists(_filePath))
            return Array.Empty<InventoryItem>();

        var json = File.ReadAllText(_filePath);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, WriteIndented = true };
        return JsonSerializer.Deserialize<List<InventoryItem>>(json, options) ?? new List<InventoryItem>();
    }

    public void StoreInventory(IEnumerable<InventoryItem> items)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(items, options);
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath) ?? string.Empty);
        File.WriteAllText(_filePath, json);
    }
}

