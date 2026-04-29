# Test Matrix

| Тип тесту | Об'єкт | Що перевіряється |
|---|---|---|
| Unit | `PharmacyService` | Додати замовлення, перевірка знижок, обробка рецепта |
| Unit | `InventoryRepository` | Пошук, оновлення запасів, збереження стану |
| Integration | `JsonPersistenceService` | Запис та зчитування даних з JSON |
| Negative | `Order` / `Prescription` | Невірний GUID, недостатній запас, некоректні дані |
