using mini_project_pharmacy.Application;
using mini_project_pharmacy.Domain.Contracts;
using mini_project_pharmacy.Domain.Models;
using mini_project_pharmacy.Infrastructure;

var dataPath = Path.Combine(AppContext.BaseDirectory, "data", "inventory.json");
var persistence = new JsonPersistenceService(dataPath);
var repository = new InventoryRepository(persistence);
var pricingStrategy = new InsurancePricingStrategy(0.15m);
var pharmacyService = new PharmacyService(repository, pricingStrategy);

EnsureSeedData(repository);

while (true)
{
    Console.Clear();
    Console.WriteLine("=== Аптечна система Capstone ===\n");
    Console.WriteLine("1. Показати звіт по запасах");
    Console.WriteLine("2. Обробити рецептурне замовлення");
    Console.WriteLine("3. Поповнити медикаменти");
    Console.WriteLine("4. Показати зведення по складу");
    Console.WriteLine("0. Вихід");
    Console.Write("Виберіть опцію: ");

    var choice = Console.ReadLine();
    Console.WriteLine();

    switch (choice)
    {
        case "1": ShowInventory(); break;
        case "2": ProcessOrder(); break;
        case "3": Restock(); break;
        case "4": ShowSummary(); break;
        case "0": return;
        default: Console.WriteLine("Невірний вибір. Натисніть Enter."); Console.ReadLine(); break;
    }
}

void EnsureSeedData(InventoryRepository repository)
{
    if (repository.GetAllItems().Any())
        return;

    var sample = new[]
    {
        new InventoryItem(new Medicine("Парацетамол", "500 мг", "Анальгетик", 12.5m, false, DateTime.UtcNow.AddMonths(18)), 120, 20, 100),
        new InventoryItem(new Medicine("Амоксицилін", "250 мг", "Антибіотик", 43.0m, true, DateTime.UtcNow.AddMonths(12)), 30, 10, 50),
        new InventoryItem(new Medicine("Сироп від кашлю", "100 мл", "Респіраторний", 75.0m, false, DateTime.UtcNow.AddMonths(24)), 18, 5, 30),
        new InventoryItem(new Medicine("Інсулін", "10 мл", "Ендокринологія", 320.0m, true, DateTime.UtcNow.AddMonths(9)), 10, 5, 20),
        new InventoryItem(new Medicine("Вітамін D", "1000 IU", "Добавка", 22.0m, false, DateTime.UtcNow.AddMonths(36)), 45, 10, 40)
    };

    foreach (var item in sample)
        repository.AddOrUpdateItem(item);

    repository.Save();
}

void ShowInventory()
{
    var report = pharmacyService.GetInventoryReport();
    Console.WriteLine("Звіт по запасах:\n");
    foreach (var line in report.Lines)
    {
        Console.WriteLine($"{line.MedicineName,-20} {line.Quantity,5} шт | {line.Status,-8} | Прострочено: {line.IsExpired}");
    }
    Console.WriteLine("\nНатисніть Enter, щоб продовжити...");
    Console.ReadLine();
}

void ProcessOrder()
{
    var customer = new Customer("Андрій Шевченко", isInsured: true, contactEmail: "andriy.shevchenko@example.com");

    Console.WriteLine("Для оформлення замовлення введіть GUID медикаменту та кількість.");
    Console.Write("GUID медикаменту: ");
    if (!Guid.TryParse(Console.ReadLine(), out var medicineId))
    {
        Console.WriteLine("Невірний формат GUID. Натисніть Enter.");
        Console.ReadLine();
        return;
    }

    var prescription = new Prescription(customer.Id, medicineId, "Лікар Катерина Іванова", DateTime.UtcNow.AddDays(14), DateTime.UtcNow.AddDays(30));
    var lines = new List<(Guid medicineId, int quantity)>();

    Console.Write("Кількість: ");
    if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity <= 0)
    {
        Console.WriteLine("Невірна кількість. Натисніть Enter.");
        Console.ReadLine();
        return;
    }

    lines.Add((medicineId, quantity));
    var result = pharmacyService.CreatePrescriptionOrder(customer, prescription, lines);
    Console.WriteLine(result.IsSuccess ? $"Успішно: {result.Message}" : $"Помилка: {result.Message}");
    if (result.IsSuccess)
    {
        Console.WriteLine($"Загальна сума: {result.Order!.TotalPrice:C}");
        Console.WriteLine($"Кількість рядків: {result.Order.Lines.Count}");
    }

    Console.WriteLine("\nНатисніть Enter, щоб продовжити...");
    Console.ReadLine();
}

void Restock()
{
    Console.Write("GUID медикаменту для поповнення: ");
    if (!Guid.TryParse(Console.ReadLine(), out var medicineId))
    {
        Console.WriteLine("Невірний GUID. Натисніть Enter.");
        Console.ReadLine();
        return;
    }

    Console.Write("Кількість для поповнення: ");
    if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity <= 0)
    {
        Console.WriteLine("Невірна кількість. Натисніть Enter.");
        Console.ReadLine();
        return;
    }

    try
    {
        pharmacyService.RestockMedicine(medicineId, quantity);
        Console.WriteLine("Медикамент успішно поповнено.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
    }

    Console.WriteLine("\nНатисніть Enter, щоб продовжити...");
    Console.ReadLine();
}

void ShowSummary()
{
    var summary = pharmacyService.GetPharmacySummary();
    Console.WriteLine($"Загальна кількість позицій: {summary.TotalItems}");
    Console.WriteLine($"Позицій з низьким запасом: {summary.LowStockItems}");
    Console.WriteLine($"Звіт сформовано: {summary.GeneratedAt:u}");
    Console.WriteLine("\nНатисніть Enter, щоб продовжити...");
    Console.ReadLine();
}

