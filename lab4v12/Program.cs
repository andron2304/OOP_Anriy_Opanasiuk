using System;
using System.Collections.Generic;

// Інтерфейс для атаки
public interface IAttack
{
    int Attack(); // метод для обчислення шкоди
}

// Абстрактний клас, що представляє базового ігрового персонажа
public abstract class Character : IAttack
{
    public string Name { get; set; }
    public int BaseDamage { get; set; }

    // Конструктор базового персонажа
    public Character(string name, int baseDamage)
    {
        Name = name;
        BaseDamage = baseDamage;
    }

    // Абстрактний метод атаки — реалізується у похідних класах
    public abstract int Attack();

    // Віртуальний метод виводу інформації про персонажа
    public virtual void ShowInfo()
    {
        Console.WriteLine($"Ім'я: {Name}, Базова шкода: {BaseDamage}");
    }
}

// Реалізація класу Воїн
public class Warrior : Character
{
    public int Strength { get; set; } // сила воїна впливає на шкоду

    public Warrior(string name, int baseDamage, int strength)
        : base(name, baseDamage)
    {
        Strength = strength;
    }

    public override int Attack()
    {
        return BaseDamage + Strength * 2; // формула шкоди
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"Воїн {Name}, базова шкода: {BaseDamage}, сила: {Strength}, атака: {Attack()}");
    }
}

// Реалізація класу Лучник
public class Archer : Character
{
    public int Agility { get; set; } // спритність лучника

    public Archer(string name, int baseDamage, int agility)
        : base(name, baseDamage)
    {
        Agility = agility;
    }

    public override int Attack()
    {
        return BaseDamage + Agility; // спритність додає до шкоди
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"Лучник {Name}, базова шкода: {BaseDamage}, спритність: {Agility}, атака: {Attack()}");
    }
}

// Клас групи персонажів (композиція)
public class Team
{
    private List<Character> members = new List<Character>();

    public void AddMember(Character character)
    {
        members.Add(character);
    }

    // Обчислення сумарної шкоди групи
    public int TotalDamage()
    {
        int total = 0;
        foreach (var member in members)
        {
            total += member.Attack();
        }
        return total;
    }

    public void ShowTeam()
    {
        Console.WriteLine("Склад групи:");
        foreach (var member in members)
        {
            member.ShowInfo();
        }
        Console.WriteLine($"Сумарна шкода групи: {TotalDamage()}");
    }
}

class Program
{
    static void Main()
    {
        // Створюємо персонажів
        Warrior warrior = new Warrior("Андрій", 10, 5);
        Archer archer = new Archer("Олег", 8, 7);

        // Створюємо групу
        Team team = new Team();
        team.AddMember(warrior);
        team.AddMember(archer);

        // Виводимо інформацію
        team.ShowTeam();
    }
}
