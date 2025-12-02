using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace IndependentWork12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // --- Налаштування експерименту ---
            // Розміри колекцій (можеш змінювати при потребі)
            int[] sizes = { 1_000_000, 5_000_000, 10_000_000 };

            // Для фіксації результатів (для звіту)
            var results = new List<PerfResult>();

            foreach (int size in sizes)
            {
                Console.WriteLine($"\n=== Експеримент для {size:N0} елементів ===");

                List<int> data = GenerateRandomList(size);

                // 1) Звичайний LINQ
                var sw = Stopwatch.StartNew();
                var seqResult = RunSequentialLinq(data);
                sw.Stop();
                long seqTime = sw.ElapsedMilliseconds;
                Console.WriteLine($"LINQ   (послідовний): {seqTime} мс, результатів: {seqResult.Count}");

                // 2) PLINQ
                sw.Restart();
                var parResult = RunParallelLinq(data);
                sw.Stop();
                long parTime = sw.ElapsedMilliseconds;
                Console.WriteLine($"PLINQ (паралельний) : {parTime} мс, результатів: {parResult.Count}");

                results.Add(new PerfResult
                {
                    Size = size,
                    SequentialMs = seqTime,
                    ParallelMs = parTime
                });
            }

            // --- Демонстрація проблеми безпеки (побічні ефекти) ---
            Console.WriteLine("\n=== Побічні ефекти та потокобезпечність у PLINQ ===");
            DemonstrateSideEffects();

            // --- Міні-звіт прямо в консолі (кратко) ---
            Console.WriteLine("\n=== Підсумкова таблиця продуктивності (мс) ===");
            foreach (var r in results)
            {
                Console.WriteLine(
                    $"N = {r.Size:N0}: LINQ = {r.SequentialMs} мс, PLINQ = {r.ParallelMs} мс, " +
                    $"різниця = {r.SequentialMs - r.ParallelMs} мс");
            }

            Console.WriteLine("\nНатисніть Enter для виходу...");
            Console.ReadLine();
        }

        // --------------------------------------------------------
        // 2. Генерація великої колекції випадкових int
        // --------------------------------------------------------
        static List<int> GenerateRandomList(int count)
        {
            var rnd = new Random();
            var list = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                // Числа від 1 до 1_000_000
                list.Add(rnd.Next(1, 1_000_001));
            }
            return list;
        }

        // --------------------------------------------------------
        // 3. Обчислювально інтенсивна операція
        //    Тут: перевірка на просте число + "важка" математика
        // --------------------------------------------------------
        static bool IsPrimeHeavy(int n)
        {
            if (n < 2) return false;
            if (n % 2 == 0 && n != 2) return false;

            // Перевірка діленням до sqrt(n)
            int limit = (int)Math.Sqrt(n);
            for (int i = 3; i <= limit; i += 2)
            {
                if (n % i == 0) return false;
            }

            // Додаткова "важка" математика (щоб навантажити CPU)
            double acc = 0;
            for (int i = 0; i < 50; i++)
            {
                acc += Math.Sqrt(n) * Math.Sin(i) * Math.Cos(i);
            }
            // acc нам не потрібний, але JIT не оптимізує все в нуль,
            // бо є невизначені тригонометричні обчислення.

            return true;
        }

        // --------------------------------------------------------
        // 4.1. Послідовний LINQ
        // --------------------------------------------------------
        static List<int> RunSequentialLinq(List<int> data)
        {
            var query =
                data
                .Where(x => x > 1_000)          // фільтр
                .Where(x => x % 2 == 1)         // тільки непарні
                .Where(x => IsPrimeHeavy(x));   // важка перевірка

            return query.ToList();
        }

        // --------------------------------------------------------
        // 4.2. Паралельний PLINQ
        // --------------------------------------------------------
        static List<int> RunParallelLinq(List<int> data)
        {
            var query =
                data
                .AsParallel()                   // перехід до PLINQ
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Where(x => x > 1_000)
                .Where(x => x % 2 == 1)
                .Where(x => IsPrimeHeavy(x));

            return query.ToList();
        }

        // --------------------------------------------------------
        // 5. Побічні ефекти та потокобезпечність
        // --------------------------------------------------------
        static void DemonstrateSideEffects()
        {
            var data = GenerateRandomList(200_000);

            // ❌ НЕБЕЗПЕЧНИЙ ВАРІАНТ: спільна змінна sum без синхронізації
            int wrongSum = 0;
            data.AsParallel().ForAll(x =>
            {
                // Це НЕ потокобезпечно: декілька потоків
                // одночасно змінюють одну змінну
                wrongSum += x;
            });

            // ✅ БЕЗПЕЧНИЙ ВАРІАНТ: використовуємо Interlocked.Add
            long safeSum = 0;
            data.AsParallel().ForAll(x =>
            {
                Interlocked.Add(ref safeSum, x);
            });

            // Для порівняння — "правильна" послідовна сума
            long seqSum = 0;
            foreach (var x in data)
            {
                seqSum += x;
            }

            Console.WriteLine($"Послідовна сума      : {seqSum}");
            Console.WriteLine($"PLINQ без синхронізації (wrongSum): {wrongSum}");
            Console.WriteLine($"PLINQ з Interlocked   (safeSum)   : {safeSum}");

            Console.WriteLine("\nКоментар:");
            Console.WriteLine("- wrongSum часто буде НЕ дорівнювати seqSum (race condition).");
            Console.WriteLine("- safeSum через Interlocked.Add збігається з seqSum.");
        }

        // --------------------------------------------------------
        // Допоміжна структура для збереження результатів
        // --------------------------------------------------------
        class PerfResult
        {
            public int Size { get; set; }
            public long SequentialMs { get; set; }
            public long ParallelMs { get; set; }
        }
    }
}