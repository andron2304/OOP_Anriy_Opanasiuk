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
            var student1 = new Student("S001", "John Doe", "john@example.com");
            var student2 = new Student("S002", "Jane Smith", "jane@example.com");
            studentRepo.Add(student1);
            studentRepo.Add(student2);

            var course1 = new Course("C001", "Math 101", "Basic Mathematics", 2);
            var course2 = new Course("C002", "Physics 101", "Basic Physics", 1);
            courseRepo.Add(course1);
            courseRepo.Add(course2);

            System.Console.WriteLine("Welcome to Student Course Registration System");
            System.Console.WriteLine("Available commands:");
            System.Console.WriteLine("1. Register student for course");
            System.Console.WriteLine("2. View student's courses");
            System.Console.WriteLine("3. View available courses");
            System.Console.WriteLine("4. Exit");

            while (true)
            {
                System.Console.Write("\nEnter command (1-4): ");
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
                        System.Console.WriteLine("Invalid command");
                        break;
                }
            }
        }

        static void RegisterStudent(CourseService service)
        {
            System.Console.Write("Enter student ID: ");
            var studentId = System.Console.ReadLine();
            System.Console.Write("Enter course ID: ");
            var courseId = System.Console.ReadLine();

            var result = service.RegisterStudentForCourse(studentId, courseId);
            if (result.IsSuccess)
            {
                System.Console.WriteLine("Registration successful!");
            }
            else
            {
                System.Console.WriteLine($"Registration failed: {result.Error}");
            }
        }

        static void ViewStudentCourses(CourseService service)
        {
            System.Console.Write("Enter student ID: ");
            var studentId = System.Console.ReadLine();

            var result = service.GetStudentCourses(studentId);
            if (result.IsSuccess)
            {
                var courses = (List<Course>)result.Data;
                System.Console.WriteLine($"Courses for student {studentId}:");
                foreach (var course in courses)
                {
                    System.Console.WriteLine($"- {course.Name} ({course.Id})");
                }
            }
            else
            {
                System.Console.WriteLine($"Error: {result.Error}");
            }
        }

        static void ViewAvailableCourses(CourseService service)
        {
            var result = service.GetAvailableCourses();
            if (result.IsSuccess)
            {
                var courses = (List<Course>)result.Data;
                System.Console.WriteLine("Available courses:");
                foreach (var course in courses)
                {
                    System.Console.WriteLine($"- {course.Name} ({course.Id}) - Capacity: {course.EnrolledStudents.Count}/{course.MaxCapacity}");
                }
            }
            else
            {
                System.Console.WriteLine($"Error: {result.Error}");
            }
        }
    }
}
