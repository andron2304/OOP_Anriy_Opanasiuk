# Система реєстрації студентів на курси

Консольний додаток для управління реєстрацією студентів на курси, побудований за шаровою архітектурою.

## Структура проєкту

```
src/
├── StudentRegistrationSystem.Domain/     # Основні сутності та інтерфейси
├── StudentRegistrationSystem.Application/ # Бізнес-логіка та сервіси
├── StudentRegistrationSystem.Infrastructure/ # Імплементації доступу до даних
└── StudentRegistrationSystem.Console/    # Інтерфейс користувача

tests/
└── StudentRegistrationSystem.Tests/      # Модульні тести

docs/
├── vision.md
├── backlog.md
├── class-diagram.md
├── sequence-diagram.md
└── iteration-1.md
```

## Як запустити

1. Перейдіть у директорію `lab34`
2. Відновіть залежності: `dotnet restore`
3. Зберіть рішення: `dotnet build`
4. Запустіть консольний додаток: `dotnet run --project src/StudentRegistrationSystem.Console`

## Доступні команди

1. Зареєструвати студента на курс
2. Переглянути курси студента
3. Переглянути доступні курси
4. Вийти

## Запуск тестів

`dotnet test`

## Архітектура

- **Domain**: Сутності (Student, Course) та інтерфейси репозиторіїв
- **Application**: Бізнес-сервіси і логіка
- **Infrastructure**: Імплементації репозиторіїв (поки що in-memory)
- **Console**: Інтерфейс користувача

## Поточні функції

- Управління студентами та курсами
- Реєстрація на курс з перевіркою доступності
- Базова валідація та обробка помилок
- Модульні тести для основної функціональності