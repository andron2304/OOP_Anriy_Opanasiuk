# Self-Audit

## Перевірка вимог курсу

### Обов'язкові основи ООП
- [x] класи і конструктори (Domain/*.cs)
- [x] інкапсуляція (private поля, public properties)
- [x] спадкування (не використано явно, але є interface-базована композиція)

### Абстракції
- [x] `IInventoryRepository` — репозиторій для роботи з інвентарем
- [x] `IPersistenceService` — абстракція збереження
- [x] `IPricingStrategy` — стратегія розрахунку ціни

### Generics і колекції
- [x] `List<T>` — збереження лік лік у `InventoryRepository`
- [x] `IReadOnlyCollection<T>` — безпечний доступ до даних
- [x] `Dictionary<Guid, InventoryItem>` — индексація по ID

### LINQ
- [x] `.Where()` — фільтрація запасів
- [x] `.Select()` — трансформація даних
- [x] `.FirstOrDefault()` — пошук по критеріям
- [x] `.Sum()` — підсумування

### Обробка помилок
- [x] `DomainException` — кастомний виняток
- [x] перевірка на null і некоректні GUID
- [x] обробка недостатнього запасу

### SOLID
- [x] Single Responsibility — кожний клас має одну відповідальність
- [x] Open/Closed — відкрито для розширення через інтерфейси
- [x] Liskov Substitution — реалізації інтерфейсів взаємозамінні
- [x] Interface Segregation — малі, сфокусовані інтерфейси
- [x] Dependency Inversion — залежність від абстракцій

### Патерни
- [x] Repository — `InventoryRepository`
- [x] Strategy — `IPricingStrategy` / `InsurancePricingStrategy`
- [x] Adapter — `JsonPersistenceService` адаптує JSON до домену

### UML
- [x] Class Diagram — `docs/class-diagram.md`
- [x] Sequence Diagram — `docs/sequence-diagram.md`

### Тестування
- [x] Unit тести — `PharmacyServiceTests.cs`
- [x] Integration тести — `InventoryPersistenceTests.cs`
- [x] 3 успішних тести

### Рефакторинг
- [x] Розділення на шари
- [x] Усунення дублювання
- [x] Переміщення логіки у правильні місця
