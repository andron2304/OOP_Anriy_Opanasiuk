using System.Text.Json;
using lab35.Domain;

namespace lab35.Infrastructure;

public class JsonDataStore
{
    private readonly string _path = "pharmacy_data.json";

    public async Task SaveAsync(IEnumerable<Medication> data)
    {
        using var stream = File.Create(_path);
        await JsonSerializer.SerializeAsync(stream, data, new JsonSerializerOptions { WriteIndented = true });
    }

    public async Task<List<Medication>> LoadAsync()
    {
        if (!File.Exists(_path)) return new List<Medication>();
        using var stream = File.OpenRead(_path);
        return await JsonSerializer.DeserializeAsync<List<Medication>>(stream) ?? new List<Medication>();
    }
}