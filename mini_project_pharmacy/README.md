# mini_project_pharmacy

Аптечна система з багатошаровою архітектурою, розробленої відповідно до вимог курсу блока 4.5.

## Етапи виконання

### Ітерація 1 (Lab 34)
**Мета**: постановка задачі, доменна модель, архітектурний каркас.

Артефакти:
- `docs/vision.md` — бачення проєкту
- `docs/backlog.md` — початковий бекло́г
- `docs/class-diagram.md` — UML діаграма класів
- `docs/sequence-diagram.md` — діаграма послідовності
- `docs/iteration-1.md` — детальний звіт
- `src/` + базові тести

### Ітерація 2 (Lab 35)
**Мета**: розширення use cases, persistence, LINQ, патерни.

Артефакти:
- `docs/iteration-2.md` — звіт
- JSON persistence у `src/mini_project_pharmacy.Infrastructure/`
- 4 use cases (inventory report, prescription order, restock, summary)
- Розширені тести

### Ітерація 3 (Lab 36)
**Мета**: quality gate, unit/integration tests, fault handling.

Артефакти:
- `TESTING.md` — стратегія тестування
- `docs/testing.md` — опис тестів
- `docs/test-strategy.md` — план тестування
- `docs/test-matrix.md` — матриця тест-кейсів
- `docs/iteration-3.md` — звіт

### Ітерація 4 (Lab 37)
**Мета**: release hardening, документація, реліз, demo.

Артефакти:
- `USER_GUIDE.md` — керівництво користувача
- `DEVELOPER_GUIDE.md` — керівництво розробника
- `CHANGELOG.md` — история змін
- `FINAL_REPORT.md` — фіналь. звіт
- `DEMO.md` — сценарії демонстрації
- `docs/syllabus-coverage.md` — охопленні теми курсу

### Self 29
**Мета**: закрити прогалини, підготування до захисту.

Артефакти:
- `docs/self-audit.md` — перевірка вимог
- `docs/extension-plan.md` — план розширень
- `docs/extension-report.md` — звіт про розширення
- `docs/defense-checklist.md` — чек-лист захисту

## Матриця інтеграції тем курсу

### Обов'язкові основи
| Тема | Реалізація | Місцезнаходження |
|------|------------|------------------|
| Класи, конструктори | `Medicine`, `InventoryItem`, `Order` | Domain/Models/ |
| Інкапсуляція | private поля, public properties | Domain/Models/ |
| Абстракції (інтерфейси) | `IInventoryRepository`, `IPersistenceService`, `IPricingStrategy` | Domain/Contracts/ |
| Generics | `List<T>`, `IReadOnlyCollection<T>` | Infrastructure/ |
| LINQ | `.Where()`, `.Select()`, `.FirstOrDefault()`, `.Sum()` | Application/ |
| Обробка помилок | `DomainException`, валідація | Domain/Exceptions/ |
| SOLID | Усі 5 принципів | Архітектура |
| Патерни | Repository, Strategy, Adapter | Infrastructure/ |
| UML | Діаграми класів і послідовності | docs/ |
| Тестування | Unit + Integration тести | tests/ |
| Рефакторинг | Розділення на шари | src/ |

### Розширення (за можливістю)
| Тема | Статус |
|------|--------|
| Делегати | Готово до розширення |
| HashSet, Queue, Stack | Готово до розширення |
| Async I/O | Готово до розширення |
| Retry-політики | Готово до розширення |
| Observer, Decorator | Готово до розширення |
| LINQ extensions | Готово до розширення |

## Опис

Pharmacy Capstone — навчальний капстоун-проєкт для блока 4.5. Це аптечна система, яка демонструє:
- доменну модель;
- розділення на шари;
- бізнес-логіку, що виходить за межі простого CRUD;
- збереження стану у файл;
- unit та integration тести;
- український консольний інтерфейс.

## Що реалізовано

### Доменні класи
- `Medicine`
- `InventoryItem`
- `Customer`
- `Prescription`
- `Order` і `OrderLine`

### Use cases
- перегляд звіту по запасах (`inventory report`)
- обробка рецептурного замовлення (`prescription order`)
- поповнення запасів (`restock`)
- зведення по складу (`summary`)

### Архітектура
- Domain: моделі, контракти, винятки
- Application: сервіс `PharmacyService`
- Infrastructure: persistence, репозиторій, стратегія ціноутворення
- Console: український CLI

### Патерни і SOLID
- Repository
- Strategy
- Adapter
- залежність від абстракцій
- інкапсуляція поведінки у доменних класах
- відкритість для розширення

## Структура репозиторію

```
mini_project_pharmacy/
├── src/
│   ├── mini_project_pharmacy.Domain/
│   ├── mini_project_pharmacy.Application/
│   ├── mini_project_pharmacy.Infrastructure/
│   └── mini_project_pharmacy.Console/
├── tests/
│   └── mini_project_pharmacy.Tests/
├── docs/
│   ├── vision.md
│   ├── backlog.md
│   ├── class-diagram.md
│   ├── sequence-diagram.md
│   ├── iteration-1.md
│   ├── iteration-2.md
│   ├── iteration-3.md
│   ├── release-plan.md
│   ├── syllabus-coverage.md
│   └── defense-qa.md
├── README.md
├── TESTING.md
├── USER_GUIDE.md
├── DEVELOPER_GUIDE.md
├── CHANGELOG.md
├── FINAL_REPORT.md
├── DEMO.md
└── mini_project_pharmacy.sln
```

## Як запустити проєкт

### Варіант 1: Visual Studio
1. Відкрийте `mini_project_pharmacy.sln`.
2. Встановіть `.NET 10 SDK`, якщо ще не встановлено.
3. Встановіть проект `mini_project_pharmacy.Console` як стартовий.
4. Запустіть проєкт.

### Варіант 2: термінал (якщо `dotnet` доступний)

```powershell
cd "<project-folder>"
dotnet restore
dotnet build mini_project_pharmacy.sln
dotnet run --project src\mini_project_pharmacy.Console\mini_project_pharmacy.Console.csproj
```

### Що очікувати

При запуску програма покаже меню:
- 1 — Показати звіт по запасах
- 2 — Обробити рецептурне замовлення
- 3 — Поповнити медикаменти
- 4 — Показати зведення по складу
- 0 — Вихід

## Тестування

### Запуск тестів

```powershell
dotnet test mini_project_pharmacy.sln
```

### Що тестує
- бізнес-логіку замовлення
- збереження та завантаження інвентарю
- негативні сценарії (помилковий GUID, недостатній запас)

## Демонстрація

1. Запустіть програму.
2. Оберіть `1` та подивіться звіт по запасах.
3. Оберіть `2`, введіть ID медикаменту та кількість.
4. Оберіть `3`, введіть ID та кількість для поповнення.
5. Оберіть `4` для зведення по складу.

## Додаткова інформація

### Формат збереження
- Стан зберігається у JSON-файл `data/inventory.json`.
- Якщо директорія `data` відсутня, файл створюється автоматично при першому запуску.

### Як додати нові медикаменти

У коді `Program.cs` є початкові дані, які додаються при порожньому складі. Для подальшого розвитку можна додати команду "додати новий медикамент".

## Етапи виконання

- Iteration 1 (Lab 34): базова доменна модель, інтерфейси, консольний каркас.
- Iteration 2 (Lab 35): JSON persistence, репозиторій, LINQ-звіти, розширені use cases.
- Iteration 3 (Lab 36): unit/integration тести, негативні сценарії, документація.
- Iteration 4 (Lab 37 / Self 29): фіналізація, стабілізація, інструкції запуску, README як єдиний документ.

## Команди для Git

```powershell
git status
git add .
git commit -m "Initial Pharmacy Capstone"
```

## Контрольні точки

- `mini_project_pharmacy.Domain` — правила і моделі
- `mini_project_pharmacy.Application` — use cases
- `mini_project_pharmacy.Infrastructure` — адаптер до JSON, репозиторій
- `mini_project_pharmacy.Console` — український інтерфейс

