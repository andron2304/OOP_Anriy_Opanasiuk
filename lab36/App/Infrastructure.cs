using System.Text.Json;
using App.Domain;
using App.Interfaces;

namespace App.Infrastructure;

public class JsonOrderRepository : IOrderRepository
{
    private readonly string _filePath;

    public JsonOrderRepository(string filePath)
    {
        _filePath = filePath;
    }

    public async Task SaveAsync(IEnumerable<Order> orders)
    {
        try
        {
            var json = JsonSerializer.Serialize(orders);
            await File.WriteAllTextAsync(_filePath, json);
        }
        catch (IOException ex)
        {
            throw new DomainException($"Помилка I/O при збереженні: {ex.Message}");
        }
    }

    public async Task<IEnumerable<Order>> LoadAsync()
    {
        if (!File.Exists(_filePath)) return new List<Order>();

        try
        {
            var json = await File.ReadAllTextAsync(_filePath);
            if (string.IsNullOrWhiteSpace(json)) return new List<Order>();
            
            return JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
        }
        catch (JsonException)
        {
            throw new DomainException("Файл пошкоджено. Неможливо прочитати дані.");
        }
    }
}