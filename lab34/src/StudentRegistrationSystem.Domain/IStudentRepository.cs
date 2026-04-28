using System.Collections.Generic;

namespace StudentRegistrationSystem.Domain
{
    public interface IStudentRepository
    {
        Student GetById(string id);
        IEnumerable<Student> GetAll();
        void Add(Student student);
        void Update(Student student);
    }
}