# Developer Guide

## Архітектура
- `src/mini_project_pharmacy.Domain` — доменні моделі й інтерфейси
- `src/mini_project_pharmacy.Application` — бізнес-логіка
- `src/mini_project_pharmacy.Infrastructure` — збереження даних, репозиторій, стратегії
- `src/mini_project_pharmacy.Console` — консольний інтерфейс

## Розширення
- Додайте нові use cases в `PharmacyService`
- Для нової persistence реалізації створіть клас, що реалізує `IPersistenceService`
- Тести додавайте в `tests/mini_project_pharmacy.Tests`

