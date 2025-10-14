using System;
using System.Collections.Generic;
namespace lab1v12
{
    // Простий клас Polynomial для прикладу
    class Polynomial
    {
        public double[] Coefficients { get; set; }

        public Polynomial(params double[] coefficients)
        {
            Coefficients = coefficients;
        }

        public override string ToString()
        {
            string result = "";
            for (int i = Coefficients.Length - 1; i >= 0; i--)
            {
                if (Coefficients[i] != 0)
                {
                    result += $"{Coefficients[i]}x^{i} ";
                    if (i > 0) result += "+ ";
                }
            }
            return result.TrimEnd();
        }
    }

    // === Клас PolynomialSet ===
    class PolynomialSet
    {
        private List<Polynomial> polynomials;

        public PolynomialSet()
        {
            polynomials = new List<Polynomial>();
        }

        // Індексатор [i]
        public Polynomial this[int index]
        {
            get
            {
                if (index >= 0 && index < polynomials.Count)
                    return polynomials[index];
                else
                    throw new IndexOutOfRangeException("Невірний індекс поліно́ма!");
            }
            set
            {
                if (index >= 0 && index < polynomials.Count)
                    polynomials[index] = value;
                else
                    throw new IndexOutOfRangeException("Невірний індекс поліно́ма!");
            }
        }

        // Оператор + для додавання нового поліно́ма
        public static PolynomialSet operator +(PolynomialSet set, Polynomial p)
        {
            set.polynomials.Add(p);
            return set;
        }

        // Метод для виведення всіх поліномів
        public void ShowAll()
        {
            Console.WriteLine("Набір поліномів:");
            for (int i = 0; i < polynomials.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {polynomials[i]}");
            }
        }
    }

    // === Демонстрація роботи ===
    class Program
    {
        static void Main()
        {
            PolynomialSet set = new PolynomialSet();

            // Створення кількох поліномів
            Polynomial p1 = new Polynomial(1, 2, 3); // 3x^2 + 2x^1 + 1
            Polynomial p2 = new Polynomial(0, -4, 5); // 5x^2 - 4x^1
            Polynomial p3 = new Polynomial(2, 0, 0, 1); // x^3 + 2

            // Додавання в набір через оператор +
            set = set + p1;
            set = set + p2;
            set = set + p3;

            // Вивід усіх поліномів
            set.ShowAll();

            // Використання індексатора
            Console.WriteLine("\nПерший поліном через індексатор:");
            Console.WriteLine(set[0]);
        }
    }
}
