using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

class Program
{
    // Назви наших файлів
    const string InputFile = "all_files_data.csv";
    const string OutputFile = "large_files_filtered.csv";
    const int RowCount = 1_000_000; // 1 мільйон рядків!

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Лабораторна робота 29 (Варіант 13: Файли) ===\n");

        // 1. Генерація великого файлу (виконається тільки якщо файлу ще немає)
        Console.WriteLine("1. Перевірка та генерація великого файлу (1 млн рядків)...");
        GenerateLargeFile();
        Console.WriteLine("   Генерація завершена!\n");

        // 2. Синхронне читання (для порівняння)
        Console.WriteLine("2. Запуск СИНХРОННОГО читання...");
        var swSync = Stopwatch.StartNew();
        ProcessSync();
        swSync.Stop();
        Console.WriteLine($"   Час синхронного читання: {swSync.ElapsedMilliseconds} мс\n");

        // 3. Асинхронне читання, обробка та запис
        Console.WriteLine("3. Запуск АСИНХРОННОГО читання та фільтрації...");
        var swAsync = Stopwatch.StartNew();
        await ProcessAsync();
        swAsync.Stop();
        Console.WriteLine($"   Час асинхронного читання та запису: {swAsync.ElapsedMilliseconds} мс\n");

        Console.WriteLine("Роботу успішно завершено! Перевір папку проєкту.");
    }

    //МЕТОД ГЕНЕРАЦІЇ ФАЙЛУ
    static void GenerateLargeFile()
    {
        // Якщо файл вже є і він великий, не генеруємо його знову, щоб не чекати
        if (File.Exists(InputFile) && new FileInfo(InputFile).Length > 1000000) return;

        using var writer = new StreamWriter(InputFile);
        writer.WriteLine("FileName,Extension,SizeBytes"); // Заголовок CSV
        
        var rand = new Random();
        for (int i = 0; i < RowCount; i++)
        {
            // Генеруємо випадковий розмір файлу від 100 байт до 50 Мегабайт
            long size = rand.Next(100, 50_000_000); 
            writer.WriteLine($"document_{i}.pdf,.pdf,{size}");
        }
    }

    //СИНХРОННИЙ МЕТОД (Просто рахуємо розмір)
    static void ProcessSync()
    {
        long totalSizeBytes = 0;
        using var reader = new StreamReader(InputFile);
        reader.ReadLine(); // Пропускаємо перший рядок (заголовок)

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            var parts = line.Split(',');
            // Беремо 3-тю колонку (розмір) і перетворюємо в число
            if (parts.Length == 3 && long.TryParse(parts[2], out long size))
            {
                totalSizeBytes += size;
            }
        }
        
        long totalMb = totalSizeBytes / (1024 * 1024);
        Console.WriteLine($"   -> Загальний розмір усіх файлів (Sync): {totalMb} МБ");
    }

    //АСИНХРОННИЙ МЕТОД (Рахуємо розмір + Фільтруємо великі файли)
    static async Task ProcessAsync()
    {
        long totalSizeBytes = 0;
        int largeFilesCount = 0;
        long limitBytes = 10_485_760; // 10 Мегабайт у байтах

        // Відкриваємо один потік для читання, інший - для запису
        using var reader = new StreamReader(InputFile);
        using var writer = new StreamWriter(OutputFile);
        
        await reader.ReadLineAsync(); // Пропускаємо заголовок в оригіналі
        await writer.WriteLineAsync("FileName,Extension,SizeBytes"); // Пишемо заголовок у новий файл

        string line;
        // Читаємо асинхронно рядок за рядком
        while ((line = await reader.ReadLineAsync()) != null)
        {
            var parts = line.Split(',');
            if (parts.Length == 3 && long.TryParse(parts[2], out long size))
            {
                totalSizeBytes += size;

                // ФІЛЬТРАЦІЯ: Якщо файл більший за 10 МБ, записуємо його в новий документ
                if (size > limitBytes)
                {
                    largeFilesCount++;
                    await writer.WriteLineAsync(line);
                }
            }
        }

        long totalMb = totalSizeBytes / (1024 * 1024);
        Console.WriteLine($"   -> Загальний розмір усіх файлів (Async): {totalMb} МБ");
        Console.WriteLine($"   -> Знайдено файлів більших за 10 МБ: {largeFilesCount}");
        Console.WriteLine($"   -> Їх успішно записано у файл: {OutputFile}");
    }
}