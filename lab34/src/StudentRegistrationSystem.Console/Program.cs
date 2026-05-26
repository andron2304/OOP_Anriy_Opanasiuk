using StudentRegistrationSystem.Application;
using StudentRegistrationSystem.Domain;
using StudentRegistrationSystem.Infrastructure;

namespace StudentRegistrationSystem.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize repositories and service
            var courseRepo = new InMemoryCourseRepository();
            var studentRepo = new InMemoryStudentRepository();
            var courseService = new CourseService(courseRepo, studentRepo);

            // Seed some data
            var student1 = new Student("S001", "Іван Іваненко", "ivan@example.com");
            var student2 = new Student("S002", "Олена Петрова", "olena@example.com");
            studentRepo.Add(student1);
            studentRepo.Add(student2);

            var course1 = new Course("C001", "Математика 101", "Базова математика", 2);
            var course2 = new Course("C002", "Фізика 101", "Базова фізика", 1);
            courseRepo.Add(course1);
            courseRepo.Add(course2);

            System.Console.WriteLine("Ласкаво просимо до системи реєстрації студентів на курси");
            System.Console.WriteLine("Доступні команди:");
            System.Console.WriteLine("1. Зареєструвати студента на курс");
            System.Console.WriteLine("2. Переглянути курси студента");
            System.Console.WriteLine("3. Переглянути доступні курси");
            System.Console.WriteLine("4. Вихід");

            while (true)
            {
                System.Console.Write("\nВведіть команду (1-4): ");
                var input = System.Console.ReadLine();

                switch (input)
                {
                    case "1":
                        RegisterStudent(courseService);
                        break;
                    case "2":
                        ViewStudentCourses(courseService);
                        break;
                    case "3":
                        ViewAvailableCourses(courseService);
                        break;
                    case "4":
                        return;
                    default:
                        System.Console.WriteLine("Невірна команда");
                        break;
                }
            }
        }

        static void RegisterStudent(CourseService service)
        {
            System.Console.Write("Введіть ID студента: ");
            var studentId = System.Console.ReadLine();
            System.Console.Write("Введіть ID курсу: ");
            var courseId = System.Console.ReadLine();

            var result = service.RegisterStudentForCourse(studentId, courseId);
            if (result.IsSuccess)
            {
                System.Console.WriteLine("Реєстрація успішна!");
            }
            else
            {
                System.Console.WriteLine($"Помилка реєстрації: {result.Error}");
            }
        }

        static void ViewStudentCourses(CourseService service)
        {
            System.Console.Write("Введіть ID студента: ");
            var studentId = System.Console.ReadLine();

            var result = service.GetStudentCourses(studentId);
            if (result.IsSuccess)
            {
                var courses = (List<Course>)result.Data;
                System.Console.WriteLine($"Курси студента {studentId}:");
                foreach (var course in courses)
                {
                    System.Console.WriteLine($"- {course.Name} ({course.Id})");
                }
            }
            else
            {
                System.Console.WriteLine($"Помилка: {result.Error}");
            }
        }

        static void ViewAvailableCourses(CourseService service)
        {
            var result = service.GetAvailableCourses();
            if (result.IsSuccess)
            {
                var courses = (List<Course>)result.Data;
                System.Console.WriteLine("Доступні курси:");
                foreach (var course in courses)
                {
                    System.Console.WriteLine($"- {course.Name} ({course.Id}) - Місць: {course.EnrolledStudents.Count}/{course.MaxCapacity}");
                }
            }
            else
            {
                System.Console.WriteLine($"Помилка: {result.Error}");
            }
        }
    }
}
