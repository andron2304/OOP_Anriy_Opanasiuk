using System;
using System.Collections.Generic;
using System.Linq;

// ----- 1. Власний делегат -----
delegate int MyOperation(int a, int b);

class Program
{
    static void Main()
    {
        // ======= КОЛЕКЦІЯ =======
        List<int> numbers = new List<int> { 5, 2, 11, 8, 14, 3, 7, 20 };

        Console.WriteLine("Початковий список:");
        Console.WriteLine(string.Join(", ", numbers));


        // ======= 2. Predicate — фільтрація парних чисел =======
        Predicate<int> isEven = x => x % 2 == 0;   // Лямбда-вираз
        List<int> evenNumbers = numbers.FindAll(isEven);

        Console.WriteLine("\nПарні числа (Predicate):");
        Console.WriteLine(string.Join(", ", evenNumbers));


        // ======= 3. Comparison — сортування =======
        Comparison<int> comparison = (a, b) => a.CompareTo(b);
        numbers.Sort(comparison);

        Console.WriteLine("\nВідсортований список (Comparison):");
        Console.WriteLine(string.Join(", ", numbers));


        // ======= 4. Func — обчислення суми =======
        Func<List<int>, int> sumFunc = list => list.Sum();

        int sum = sumFunc(numbers);

        Console.WriteLine("\nСума всіх елементів (Func): " + sum);


        // ======= 5. Анонімний метод =======
        MyOperation add = delegate (int a, int b)
        {
            return a + b;
        };

        Console.WriteLine("\nАнонімний метод MyOperation (5 + 7): " + add(5, 7));


        // ======= 6. Лямбда-вираз для MyOperation =======
        MyOperation multiply = (a, b) => a * b;

        Console.WriteLine("Lambda MyOperation (5 * 7): " + multiply(5, 7));


        // ======= 7. Action =======
        Action<string> show = msg => Console.WriteLine("\n[Action] " + msg);
        show("Лабораторна №6 — виконано!");


        // ======= 8. LINQ обробка =======
        var processed =
            numbers
            .Where(n => n > 5)        // фільтр
            .Select(n => n * 2)       // вибірка
            .OrderBy(n => n);         // сортування

        Console.WriteLine("\nLINQ (n > 5) * 2 → sort:");
        Console.WriteLine(string.Join(", ", processed));
    }
}