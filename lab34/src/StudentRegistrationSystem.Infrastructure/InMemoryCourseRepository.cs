using System.Collections.Generic;
using StudentRegistrationSystem.Domain;

namespace StudentRegistrationSystem.Infrastructure
{
    public class InMemoryCourseRepository : ICourseRepository
    {
        private readonly Dictionary<string, Course> _courses = new Dictionary<string, Course>();

        public Course GetById(string id)
        {
            _courses.TryGetValue(id, out var course);
            return course;
        }

        public IEnumerable<Course> GetAll()
        {
            return _courses.Values;
        }

        public void Add(Course course)
        {
            _courses[course.Id] = course;
        }

        public void Update(Course course)
        {
            _courses[course.Id] = course;
        }
    }
}