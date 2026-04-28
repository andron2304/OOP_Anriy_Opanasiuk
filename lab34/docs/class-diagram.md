# Class Diagram

```mermaid
classDiagram
    class Student {
        +string Id
        +string Name
        +string Email
        +List<Course> RegisteredCourses
        +RegisterForCourse(Course course)
        +UnregisterFromCourse(Course course)
    }

    class Course {
        +string Id
        +string Name
        +string Description
        +int MaxCapacity
        +List<Student> EnrolledStudents
        +bool IsAvailable()
        +bool EnrollStudent(Student student)
    }

    class ICourseRepository {
        +Course GetById(string id)
        +IEnumerable<Course> GetAll()
        +void Add(Course course)
        +void Update(Course course)
    }

    class IStudentRepository {
        +Student GetById(string id)
        +IEnumerable<Student> GetAll()
        +void Add(Student student)
        +void Update(Student student)
    }

    class CourseService {
        +ICourseRepository CourseRepository
        +IStudentRepository StudentRepository
        +Result RegisterStudentForCourse(string studentId, string courseId)
        +Result UnregisterStudentFromCourse(string studentId, string courseId)
    }

    class InMemoryCourseRepository {
        +Dictionary<string, Course> courses
    }

    class InMemoryStudentRepository {
        +Dictionary<string, Student> students
    }

    Student ||--o Course : registered for
    Course ||--o Student : enrolled
    CourseService --> ICourseRepository
    CourseService --> IStudentRepository
    ICourseRepository <|.. InMemoryCourseRepository
    IStudentRepository <|.. InMemoryStudentRepository
```