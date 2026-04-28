using System.Collections.Generic;

namespace StudentRegistrationSystem.Domain
{
    public interface ICourseRepository
    {
        Course GetById(string id);
        IEnumerable<Course> GetAll();
        void Add(Course course);
        void Update(Course course);
    }
}