using System;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

// 1. Створюємо клас із 5 властивостями
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Major { get; set; }
    public double AverageGrade { get; set; }
}

class Program
{
    static void Main()
    {
        // Створюємо об'єкт студента
        var student = new Student 
        { 
            Id = 101, 
            Name = "Андрій", 
            Age = 18, 
            Major = "Інженерія програмного забезпечення", 
            AverageGrade = 95.5 
        };

        Console.WriteLine("Починаємо серіалізацію...\n");

        // --- 2. JSON Серіалізація ---
        var jsonOptions = new JsonSerializerOptions { WriteIndented = true }; // Щоб було красиво відформатовано
        string jsonResult = JsonSerializer.Serialize(student, jsonOptions);
        
        // Зберігаємо у файл і виводимо на екран
        File.WriteAllText("student.json", jsonResult);
        Console.WriteLine("=== Результат JSON ===");
        Console.WriteLine(jsonResult);


        // --- 3. XML Серіалізація ---
        var xmlSerializer = new XmlSerializer(typeof(Student));
        string xmlResult = "";
        
        using (var stringWriter = new StringWriter())
        {
            xmlSerializer.Serialize(stringWriter, student);
            xmlResult = stringWriter.ToString();
        }

        // Зберігаємо у файл і виводимо на екран
        File.WriteAllText("student.xml", xmlResult);
        Console.WriteLine("\n=== Результат XML ===");
        Console.WriteLine(xmlResult);
        
        Console.WriteLine("\nФайли student.json та student.xml успішно створено у папці проєкту!");
    }
}