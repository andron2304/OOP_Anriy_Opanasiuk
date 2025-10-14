using System;
namespace lab1v1
{
    // Клас Song
    class Song
    {
        // Приватні поля
        private string title;
        private string artist;
        private double duration; // тривалість пісні в секундах

        // Властивість для доступу до тривалості
        public double Duration
        {
            get { return duration; }
            set
            {
                if (value > 0)
                    duration = value;
                else
                    duration = 0;
            }
        }

        // Конструктор із параметрами
        public Song(string title, string artist, double duration)
        {
            this.title = title;
            this.artist = artist;
            Duration = duration;
            Console.WriteLine($"Створено об'єкт Song: {title} - {artist}");
        }

        // Метод Play()
        public void Play()
        {
            Console.WriteLine($"🎵 Відтворюється пісня: \"{title}\" виконавець {artist}, тривалість {duration} сек.");
        }

        // Деструктор (демонстраційний)
        ~Song()
        {
            Console.WriteLine($"Об'єкт Song \"{title}\" видаляється з пам’яті.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Лабораторна робота №1 ===");
            Console.WriteLine("Тема: Класи, об’єкти, конструктори та деструктори\n");

            // Створення об’єктів класу Song
            Song song1 = new Song("Shape of You", "Ed Sheeran", 233);
            Song song2 = new Song("Blinding Lights", "The Weeknd", 200);
            Song song3 = new Song("Ой у лузі червона калина", "Українська народна", 180);

            // Виклик методів
            song1.Play();
            song2.Play();
            song3.Play();

            Console.WriteLine("\nРоботу завершено. Натисніть будь-яку клавішу...");
            Console.ReadKey();
        }
    }
}
