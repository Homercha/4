using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

class WordFrequencyCounter
{
    static void Main(string[] args)
    {
        // Установка кодировки UTF-8 для консоли
        Console.OutputEncoding = Encoding.UTF8;

        try
        {
            string filePath;

            // Проверяем, передан ли путь к файлу в аргументах
            if (args.Length > 0)
            {
                filePath = args[0];
            }
            else
            {
                Console.WriteLine("Введите путь к текстовому файлу или перетащите его сюда:");
                filePath = Console.ReadLine()?.Trim('"');
            }

            // Проверяем, существует ли файл
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                Console.WriteLine($"Файл не найден по указанному пути: {filePath}");
                return;
            }

            Console.WriteLine($"Обрабатывается файл: {filePath}");

            // Чтение текста из файла
            string text = File.ReadAllText(filePath, Encoding.UTF8);

            // Очистка текста и приведение к нижнему регистру
            string cleanedText = Regex.Replace(text.ToLower(), @"[^\w\s]", "");

            // Разбиение на слова
            string[] words = cleanedText.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Подсчет количества слов
            var wordCounts = words.GroupBy(word => word)
                                  .ToDictionary(group => group.Key, group => group.Count());

            // Сортировка по убыванию частоты
            var sortedWordCounts = wordCounts.OrderByDescending(pair => pair.Value);

            // Вывод результатов в консоль
            Console.WriteLine("\nСтатистика слов (по убыванию частоты):");
            foreach (var pair in sortedWordCounts)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }

            // Сохранение результатов
            string outputFilePath = Path.Combine(Path.GetDirectoryName(filePath), "WordStatistics.txt");
            File.WriteAllLines(outputFilePath, sortedWordCounts.Select(pair => $"{pair.Key}: {pair.Value}"));

            Console.WriteLine($"\nСтатистика сохранена в файл: {outputFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
