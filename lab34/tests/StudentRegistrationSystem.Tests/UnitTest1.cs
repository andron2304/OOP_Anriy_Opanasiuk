using StudentRegistrationSystem.Domain;
using StudentRegistrationSystem.Application;
using StudentRegistrationSystem.Infrastructure;
using Xunit;

namespace StudentRegistrationSystem.Tests;

public class UnitTest1
{
    [Fact]
    public void Student_Creation_WithValidData_ShouldSucceed()
    {
        var student = new Student("S001", "John Doe", "john@example.com");

        Assert.Equal("S001", student.Id);
        Assert.Equal("John Doe", student.Name);
        Assert.Equal("john@example.com", student.Email);
        Assert.Empty(student.RegisteredCourses);
    }

    [Fact]
    public void Student_Creation_WithEmptyId_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => new Student("", "John Doe", "john@example.com"));
    }

    [Fact]
    public void Course_Creation_WithValidData_ShouldSucceed()
    {
        var course = new Course("C001", "Math 101", "Basic Math", 30);

        Assert.Equal("C001", course.Id);
        Assert.Equal("Math 101", course.Name);
        Assert.Equal(30, course.MaxCapacity);
        Assert.True(course.IsAvailable());
    }

    [Fact]
    public void Course_EnrollStudent_WhenAvailable_ShouldSucceed()
    {
        var course = new Course("C001", "Math 101", "Basic Math", 2);
        var student = new Student("S001", "John", "john@example.com");

        var result = course.EnrollStudent(student);

        Assert.True(result);
        Assert.Contains(student, course.EnrolledStudents);
    }

    [Fact]
    public void Course_EnrollStudent_WhenFull_ShouldFail()
    {
        var course = new Course("C001", "Math 101", "Basic Math", 1);
        var student1 = new Student("S001", "John", "john@example.com");
        var student2 = new Student("S002", "Jane", "jane@example.com");

        course.EnrollStudent(student1);
        var result = course.EnrollStudent(student2);

        Assert.False(result);
        Assert.DoesNotContain(student2, course.EnrolledStudents);
    }

    [Fact]
    public void CourseService_RegisterStudentForCourse_ShouldSucceed()
    {
        var courseRepo = new InMemoryCourseRepository();
        var studentRepo = new InMemoryStudentRepository();
        var service = new CourseService(courseRepo, studentRepo);

        var student = new Student("S001", "John", "john@example.com");
        var course = new Course("C001", "Math", "Math course", 10);

        studentRepo.Add(student);
        courseRepo.Add(course);

        var result = service.RegisterStudentForCourse("S001", "C001");

        Assert.True(result.IsSuccess);
        Assert.Contains(course, student.RegisteredCourses);
        Assert.Contains(student, course.EnrolledStudents);
    }

    [Fact]
    public void CourseService_RegisterStudentForNonExistentCourse_ShouldFail()
    {
        var courseRepo = new InMemoryCourseRepository();
        var studentRepo = new InMemoryStudentRepository();
        var service = new CourseService(courseRepo, studentRepo);

        var student = new Student("S001", "John", "john@example.com");
        studentRepo.Add(student);

        var result = service.RegisterStudentForCourse("S001", "C999");

        Assert.False(result.IsSuccess);
        Assert.Equal("Course not found", result.Error);
    }
}
