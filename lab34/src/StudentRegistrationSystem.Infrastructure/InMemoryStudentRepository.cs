using System.Collections.Generic;
using StudentRegistrationSystem.Domain;

namespace StudentRegistrationSystem.Infrastructure
{
    public class InMemoryStudentRepository : IStudentRepository
    {
        private readonly Dictionary<string, Student> _students = new Dictionary<string, Student>();

        public Student GetById(string id)
        {
            _students.TryGetValue(id, out var student);
            return student;
        }

        public IEnumerable<Student> GetAll()
        {
            return _students.Values;
        }

        public void Add(Student student)
        {
            _students[student.Id] = student;
        }

        public void Update(Student student)
        {
            _students[student.Id] = student;
        }
    }
}