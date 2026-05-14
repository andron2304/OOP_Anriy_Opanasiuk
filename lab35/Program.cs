using lab35.Domain;
using lab35.Infrastructure;
using lab35.Services;

var store = new JsonDataStore();
var service = new PharmacyService();

// Завантаження даних при старті
var loadedData = await store.LoadAsync();
service.Refresh(loadedData);

Console.WriteLine("=== АПТЕЧНА СИСТЕМА (LAB 35) ===");

while (true)
{
    Console.WriteLine("\n1. Додати ліки | 2. Список (LINQ) | 3. Зберегти | 4. Вихід");
    var choice = Console.ReadLine();

    if (choice == "1")
    {
        Console.Write("Назва: "); string name = Console.ReadLine()!;
        Console.Write("Ціна: "); decimal price = decimal.Parse(Console.ReadLine()!);
        Console.Write("Рецептурний? (y/n): "); bool presc = Console.ReadLine() == "y";

        var med = new Medication { Name = name, BasePrice = price, RequiresPrescription = presc };
        loadedData.Add(med);
        service.Refresh(loadedData);
        Console.WriteLine("Додано!");
    }
    else if (choice == "2")
    {
        Console.WriteLine("Всі товари (сортування за ціною):");
        foreach (var m in service.GetSortedByPrice())
            Console.WriteLine($"{m.Name} - {m.BasePrice} грн (Рецепт: {m.RequiresPrescription})");
        
        Console.WriteLine($"\nЗагальна вартість складу: {service.GetTotalInventoryValue()} грн");
    }
    else if (choice == "3")
    {
        await store.SaveAsync(service.GetAll());
        Console.WriteLine("Дані збережено у pharmacy_data.json!");
    }
    else if (choice == "4") break;
}