# Test Strategy

## Підхід
- Використовуємо `xUnit` для unit-тестів.
- Тестуємо сервіс `PharmacyService`, логіку `Pharmacy` та persistence.
- Дозволяємо окремо перевіряти позитивні/негативні сценарії.

## Архітектура тестів
- Unit тести для бізнес-логіки
- Integration тести для `JsonPersistenceService`
- Ізольовані тести для `InventoryRepository`
