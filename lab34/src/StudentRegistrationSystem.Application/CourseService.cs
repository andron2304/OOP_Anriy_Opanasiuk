using System.Linq;
using StudentRegistrationSystem.Domain;

namespace StudentRegistrationSystem.Application
{
    public class CourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;

        public CourseService(ICourseRepository courseRepository, IStudentRepository studentRepository)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
        }

        public Result RegisterStudentForCourse(string studentId, string courseId)
        {
            var student = _studentRepository.GetById(studentId);
            if (student == null) return Result.Failure("Student not found");

            var course = _courseRepository.GetById(courseId);
            if (course == null) return Result.Failure("Course not found");

            if (!course.IsAvailable()) return Result.Failure("Course is full");

            try
            {
                student.RegisterForCourse(course);
                course.EnrollStudent(student);
                _studentRepository.Update(student);
                _courseRepository.Update(course);
                return Result.Success();
            }
            catch (InvalidOperationException ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public Result UnregisterStudentFromCourse(string studentId, string courseId)
        {
            var student = _studentRepository.GetById(studentId);
            if (student == null) return Result.Failure("Student not found");

            var course = _courseRepository.GetById(courseId);
            if (course == null) return Result.Failure("Course not found");

            try
            {
                student.UnregisterFromCourse(course);
                course.UnenrollStudent(student);
                _studentRepository.Update(student);
                _courseRepository.Update(course);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public Result GetStudentCourses(string studentId)
        {
            var student = _studentRepository.GetById(studentId);
            if (student == null) return Result.Failure("Student not found");

            return Result.Success(student.RegisteredCourses);
        }

        public Result GetAvailableCourses()
        {
            var courses = _courseRepository.GetAll();
            var availableCourses = courses.Where(c => c.IsAvailable()).ToList();
            return Result.Success(availableCourses);
        }
    }
}