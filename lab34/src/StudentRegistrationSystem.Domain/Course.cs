using System;
using System.Collections.Generic;

namespace StudentRegistrationSystem.Domain
{
    public class Course
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int MaxCapacity { get; private set; }
        public List<Student> EnrolledStudents { get; private set; }

        public Course(string id, string name, string description, int maxCapacity)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id cannot be empty");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty");
            if (maxCapacity <= 0) throw new ArgumentException("Max capacity must be positive");

            Id = id;
            Name = name;
            Description = description;
            MaxCapacity = maxCapacity;
            EnrolledStudents = new List<Student>();
        }

        public bool IsAvailable()
        {
            return EnrolledStudents.Count < MaxCapacity;
        }

        public bool EnrollStudent(Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));
            if (!IsAvailable()) return false;
            if (EnrolledStudents.Contains(student)) return false;

            EnrolledStudents.Add(student);
            return true;
        }

        public void UnenrollStudent(Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));
            EnrolledStudents.Remove(student);
        }
    }
}