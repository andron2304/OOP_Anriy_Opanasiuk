using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// --- 1. Класи предметної області ---

public class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Repetitions { get; set; } // Кількість повторень
    public double WeightKg { get; set; } // Вага в кг
}

public class Workout
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Date { get; set; }
    public List<Exercise> Exercises { get; set; } = new List<Exercise>();
}

// --- 2. Репозиторій ---

public class WorkoutRepository
{
    private List<Workout> _workouts = new List<Workout>();

    // Додавання
    public void Add(Workout workout)
    {
        _workouts.Add(workout);
    }

    // Отримання всіх
    public List<Workout> GetAll()
    {
        return _workouts;
    }

    // Пошук за ID
    public Workout GetById(int id)
    {
        return _workouts.FirstOrDefault(w => w.Id == id);
    }

    // Асинхронне збереження у файл JSON
    public async Task SaveToFileAsync(string filename)
    {
        // Налаштування для красивого форматування тексту у файлі
        var options = new JsonSerializerOptions 
        { 
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Для кирилиці
        };

        await using FileStream createStream = File.Create(filename);
        await JsonSerializer.SerializeAsync(createStream, _workouts, options);
    }

    // Асинхронне завантаження з файлу JSON
    public async Task LoadFromFileAsync(string filename)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("Файл не знайдено!");
            return;
        }

        await using FileStream openStream = File.OpenRead(filename);
        var loadedWorkouts = await JsonSerializer.DeserializeAsync<List<Workout>>(openStream);
        
        if (loadedWorkouts != null)
        {
            _workouts = loadedWorkouts;
        }
    }
}

// --- 3. Демонстрація роботи (Main) ---

class Program
{
    static async Task Main(string[] args)
    {
        string filename = "workouts.json";
        var repository = new WorkoutRepository();

        Console.WriteLine("1. Створення об'єктів");
        
        var workout1 = new Workout
        {
            Id = 1,
            Title = "Тренування грудей та трицепсу",
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            Exercises = new List<Exercise>
            {
                new Exercise { Id = 1, Name = "Жим лежачи", Repetitions = 10, WeightKg = 80 },
                new Exercise { Id = 2, Name = "Віджимання на брусах", Repetitions = 15, WeightKg = 0 }
            }
        };

        var workout2 = new Workout
        {
            Id = 2,
            Title = "Тренування спини",
            Date = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"),
            Exercises = new List<Exercise>
            {
                new Exercise { Id = 3, Name = "Підтягування", Repetitions = 12, WeightKg = 0 },
                new Exercise { Id = 4, Name = "Тяга штанги в нахилі", Repetitions = 10, WeightKg = 60 }
            }
        };

        repository.Add(workout1);
        repository.Add(workout2);
        Console.WriteLine("Тренування успішно створені та додані в репозиторій.\n");


        Console.WriteLine("2. Збереження у JSON");
        await repository.SaveToFileAsync(filename);
        Console.WriteLine($"Дані успішно збережено у файл {filename}\n");


        Console.WriteLine("3. Завантаження з JSON");
        // Створюємо новий порожній репозиторій, щоб довести, що дані дійсно читаються з файлу
        var newRepository = new WorkoutRepository();
        await newRepository.LoadFromFileAsync(filename);
        Console.WriteLine("Дані успішно завантажено з файлу.\n");


        Console.WriteLine("4. Виведення результату");
        var allWorkouts = newRepository.GetAll();
        
        foreach (var workout in allWorkouts)
        {
            Console.WriteLine($"[{workout.Id}] {workout.Title} ({workout.Date})");
            foreach (var exercise in workout.Exercises)
            {
                Console.WriteLine($"   - {exercise.Name}: {exercise.Repetitions} разів, вага: {exercise.WeightKg} кг");
            }
        }

        Console.WriteLine("\n=== 5. Перевірка методу GetById(2) ===");
        var specificWorkout = newRepository.GetById(2);
        if (specificWorkout != null)
        {
            Console.WriteLine($"Знайдено: {specificWorkout.Title}");
        }
    }
}