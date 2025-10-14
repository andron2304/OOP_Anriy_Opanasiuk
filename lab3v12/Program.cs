using System;
using System.Collections.Generic;
namespace lab1v12
{
    // Базовий клас Course
    class Course
    {
        public string Name { get; set; }
        public int Credits { get; set; }

        public Course(string name, int credits)
        {
            Name = name;
            Credits = credits;
        }

        public virtual void ShowInfo()
        {
            Console.WriteLine($"Курс: {Name}, Кредити: {Credits}");
        }
    }

    // Похідний клас MathCourse
    class MathCourse : Course
    {
        public string Level { get; set; }

        public MathCourse(string name, int credits, string level)
            : base(name, credits)
        {
            Level = level;
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"Математичний курс: {Name}, Кредити: {Credits}, Рівень: {Level}");
        }
    }

    // Похідний клас HistoryCourse
    class HistoryCourse : Course
    {
        public string Period { get; set; }

        public HistoryCourse(string name, int credits, string period)
            : base(name, credits)
        {
            Period = period;
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"Історичний курс: {Name}, Кредити: {Credits}, Період: {Period}");
        }
    }

    // Клас StudentCourses для обчислення загальних даних
    class StudentCourses
    {
        private List<Course> courses;

        public StudentCourses()
        {
            courses = new List<Course>();
        }

        // Додавання курсу
        public void AddCourse(Course course)
        {
            courses.Add(course);
        }

        // Загальна кількість кредитів
        public int TotalCredits()
        {
            int sum = 0;
            foreach (var c in courses)
                sum += c.Credits;
            return sum;
        }

        // Середнє навантаження
        public double AverageCredits()
        {
            if (courses.Count == 0) return 0;
            return (double)TotalCredits() / courses.Count;
        }

        // Показати всі курси
        public void ShowAll()
        {
            Console.WriteLine("Список курсів студента:");
            foreach (var c in courses)
                c.ShowInfo();

            Console.WriteLine($"\nЗагальна кількість кредитів: {TotalCredits()}");
            Console.WriteLine($"Середнє навантаження: {AverageCredits():F2}");
        }
    }

    // Головний клас програми
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Лабораторна робота №1 ===");
            Console.WriteLine("Тема: Університетські курси\n");

            // Створення курсів
            MathCourse math = new MathCourse("Лінійна алгебра", 5, "Базовий рівень");
            HistoryCourse history = new HistoryCourse("Історія України", 4, "XIX століття");
            Course programming = new Course("Програмування на C#", 6);

            // Створення списку курсів студента
            StudentCourses student = new StudentCourses();
            student.AddCourse(math);
            student.AddCourse(history);
            student.AddCourse(programming);

            // Вивід інформації
            student.ShowAll();

            Console.WriteLine("\nНатисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }
    }
}
