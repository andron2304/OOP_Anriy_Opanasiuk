```markdown
# Звіт до лабораторної роботи №28

**Тема:** Серіалізація об’єктів у JSON  
**Варіант:** 13 (Тренування: Exercise, Workout, WorkoutRepository)  
**Студент:** Опанасюк Андрій  

## 1. Мета роботи
Навчитися серіалізувати та десеріалізувати складні об’єкти у форматі JSON, зберігати дані у файли та завантажувати їх з використанням асинхронних методів (`async/await`).

## 2. Предметна область (Варіант 13)
Було створено класи для відображення тренувань та конкретних вправ у них. Об'єкт `Workout` містить у собі список об'єктів `Exercise` (складний об'єкт).

```csharp
public class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Repetitions { get; set; }
    public double WeightKg { get; set; }
}

public class Workout
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Date { get; set; }
    public List<Exercise> Exercises { get; set; } = new List<Exercise>();
}
```

## 3. Реалізовані методи репозиторію
Для управління даними було створено клас `WorkoutRepository`, який містить наступні методи:
* `Add(Workout workout)` — додавання нового тренування.
* `GetAll()` — отримання списку всіх тренувань.
* `GetById(int id)` — пошук конкретного тренування за його ідентифікатором.
* `SaveToFileAsync(string filename)` — **асинхронне** збереження списку тренувань у JSON-файл за допомогою `JsonSerializer.SerializeAsync`.
* `LoadFromFileAsync(string filename)` — **асинхронне** завантаження та десеріалізація даних із файлу за допомогою `JsonSerializer.DeserializeAsync`.

## 4. Результат роботи програми (зміст файлу workouts.json)
Після створення об'єктів у методі `Main` та виклику методу `SaveToFileAsync`, програма успішно згенерувала наступний JSON-файл:

```json
[
  {
    "Id": 1,
    "Title": "Тренування грудей та трицепсу",
    "Date": "2023-11-05",
    "Exercises": [
      {
        "Id": 1,
        "Name": "Жим лежачи",
        "Repetitions": 10,
        "WeightKg": 80
      },
      {
        "Id": 2,
        "Name": "Віджимання на брусах",
        "Repetitions": 15,
        "WeightKg": 0
      }
    ]
  },
  {
    "Id": 2,
    "Title": "Тренування спини",
    "Date": "2023-11-03",
    "Exercises": [
      {
        "Id": 3,
        "Name": "Підтягування",
        "Repetitions": 12,
        "WeightKg": 0
      },
      {
        "Id": 4,
        "Name": "Тяга штанги в нахилі",
        "Repetitions": 10,
        "WeightKg": 60
      }
    ]
  }
]
```

## 5. Висновок
Під час виконання лабораторної роботи було успішно реалізовано JSON-серіалізацію та десеріалізацію складних об'єктів у C# з використанням бібліотеки `System.Text.Json`. Використання асинхронних потоків (`FileStream` разом із `async/await`) дозволяє ефективно працювати з файловою системою, не блокуючи основний потік виконання програми.
```