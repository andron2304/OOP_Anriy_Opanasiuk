# Class Diagram

Основні класи:
- `Medicine`
- `InventoryItem`
- `Customer`
- `Prescription`
- `Order`
- `OrderLine`
- `Pharmacy`
- `PharmacyService`
- `InventoryRepository`
- `JsonPersistenceService`
- `InsurancePricingStrategy`

Взаємозв'язки:
- `PharmacyService` використовує `Pharmacy`
- `Pharmacy` залежить від `IInventoryRepository` та `IPricingStrategy`
- `InventoryRepository` залежить від `IPersistenceService`
