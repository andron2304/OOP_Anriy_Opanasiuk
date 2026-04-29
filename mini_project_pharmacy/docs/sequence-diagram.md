# Sequence Diagram

## Обробка рецептурного замовлення
1. Користувач обирає опцію "Рецептурне замовлення".
2. `Program.cs` передає дані у `PharmacyService`.
3. `PharmacyService` викликає `Pharmacy.ProcessPrescriptionOrder()`.
4. `Pharmacy` запитує `InventoryRepository` та перевіряє `Prescription`.
5. `PricingStrategy` обчислює ціну.
6. `InventoryRepository` зберігає оновлений стан через `JsonPersistenceService`.
