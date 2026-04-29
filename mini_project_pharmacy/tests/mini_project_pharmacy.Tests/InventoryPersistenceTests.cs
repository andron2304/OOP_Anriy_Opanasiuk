using mini_project_pharmacy.Domain.Models;
using mini_project_pharmacy.Infrastructure;
using Xunit;

namespace mini_project_pharmacy.Tests;

public class InventoryPersistenceTests
{
    [Fact]
    public void JsonPersistenceService_CanStoreAndLoadInventory()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"pharmacy_inventory_{Guid.NewGuid()}.json");
        try
        {
            var service = new JsonPersistenceService(tempFile);
            var medicine = new Medicine("Test Item", "10mg", "Test", 15m, false, DateTime.UtcNow.AddMonths(6));
            var item = new InventoryItem(medicine, 5, 2, 10);

            service.StoreInventory(new[] { item });
            var loaded = service.LoadInventory();

            Assert.Single(loaded);
            Assert.Equal(medicine.Name, loaded.First().Medicine.Name);
            Assert.Equal(5, loaded.First().Quantity);
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }
}

