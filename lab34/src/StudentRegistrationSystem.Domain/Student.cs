using System;
using System.Collections.Generic;

namespace StudentRegistrationSystem.Domain
{
    public class Student
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public List<Course> RegisteredCourses { get; private set; }

        public Student(string id, string name, string email)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id cannot be empty");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty");

            Id = id;
            Name = name;
            Email = email;
            RegisteredCourses = new List<Course>();
        }

        public void RegisterForCourse(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
            if (RegisteredCourses.Contains(course)) throw new InvalidOperationException("Already registered for this course");

            RegisteredCourses.Add(course);
        }

        public void UnregisterFromCourse(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
            RegisteredCourses.Remove(course);
        }
    }
}