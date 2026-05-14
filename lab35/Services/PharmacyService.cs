using lab35.Domain;

namespace lab35.Services;

public class PharmacyService
{
    // Використання спеціалізованої колекції Dictionary
    private Dictionary<Guid, Medication> _inventory = new();

    public void Refresh(List<Medication> items) => _inventory = items.ToDictionary(x => x.Id);
    public List<Medication> GetAll() => _inventory.Values.ToList();

    // LINQ 1: Фільтрація (Тільки ліки за рецептом)
    public IEnumerable<Medication> GetPrescriptionOnly() => 
        _inventory.Values.Where(m => m.RequiresPrescription);

    // LINQ 2: Сортування за ціною
    public IEnumerable<Medication> GetSortedByPrice() => 
        _inventory.Values.OrderBy(m => m.BasePrice);

    // LINQ 3: Агрегація (Загальна вартість складу)
    public decimal GetTotalInventoryValue() => 
        _inventory.Values.Sum(m => m.BasePrice);

    // LINQ 4: Групування за статусом
    public object GetStatusReport() => 
        _inventory.Values.GroupBy(m => m.Status).Select(g => new { Status = g.Key, Count = g.Count() });
}