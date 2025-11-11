using System;
using System.Text;
using System.Text.RegularExpressions;

public class Program
{
    public static void Main()
    {
        // Встановлюємо кодування консолі для коректного відображення українських символів
        Console.OutputEncoding = Encoding.UTF8;

        // Заданий текст, доповнений для демонстрації нових можливостей
        string text = "Інформатика - це фундаментальна наука. Ми любимо вивчати інформатику. " +
                      "Інформатика відкриває нові горизонти. Чи знаєте ви, що інформатика є ключовою для технологій? " +
                      "Це просто чудово. Інформатика допомагає вирішувати складні задачі. " +
                      "Зустріч відбудеться 25.12.2023. Наш сервер має адресу 192.168.1.1, а шлюз - 10.0.0.1.";

        // 1. Завдання: Скільки речень починається зі слова "Інформатика"
        string sentencePattern = @"(?:^|\.\s*|\?\s*|!\s*)Інформатика";
        AnalyzeText(text, "Речення, що починаються зі слова 'Інформатика'", sentencePattern);

        // 2. Завдання: Знайти всі слова, що починаються з великої літери
        // \p{Lu} - будь-яка велика літера в Unicode
        // \p{L}* - нуль або більше будь-яких літер в Unicode
        // \b - межа слова
        string capitalizedWordPattern = @"\b\p{Lu}\p{L}*\b";
        AnalyzeText(text, "Слова, що починаються з великої літери", capitalizedWordPattern);

        // 3. Практичне завдання: пошук дат та IP-адрес
        string datePattern = @"\b\d{2}\.\d{2}\.\d{4}\b";
        AnalyzeText(text, "Дати у форматі dd.mm.yyyy", datePattern);

        string ipAddressPattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
        AnalyzeText(text, "IP-адреси", ipAddressPattern);

        // 4. Завдання: Перевірити, чи всі слова починаються з великої літери, і знайти невідповідності
        CheckWordsCapitalization(text);
    }

    /// <summary>
    /// Аналізує текст за допомогою регулярного виразу та виводить результати.
    /// </summary>
    /// <param name="inputText">Текст для аналізу.</param>
    /// <param name="description">Опис того, що шукаємо.</param>
    /// <param name="pattern">Регулярний вираз для пошуку.</param>
    public static void AnalyzeText(string inputText, string description, string pattern)
    {
        Console.WriteLine($"--- {description} ---");

        if (string.IsNullOrEmpty(inputText))
        {
            Console.WriteLine("Помилка: Вхідний текст порожній або null.");
            Console.WriteLine();
            return;
        }

        try
        {
            // RegexOptions.Compiled прискорює виконання, якщо патерн використовується багаторазово
            MatchCollection matches = Regex.Matches(inputText, pattern, RegexOptions.Compiled);

            if (matches.Count > 0)
            {
                Console.WriteLine($"Знайдено збігів: {matches.Count}");
                Console.WriteLine("Приклади:");
                foreach (Match match in matches)
                {
                    // Виводимо чистий збіг без зайвих символів на початку
                    Console.WriteLine($"- {match.Value.TrimStart('.', '?', '!', ' ')}");
                }
            }
            else
            {
                Console.WriteLine("Збігів не знайдено.");
            }
        }
        catch (RegexMatchTimeoutException)
        {
            Console.WriteLine("Помилка: Час виконання пошуку за регулярним виразом вичерпану.");
        }
        catch (ArgumentException ex)
        {
            // Цей блок спрацює, якщо патерн синтаксично невірний
            Console.WriteLine($"Помилка: Некоректний регулярний вираз. Деталі: {ex.Message}");
        }
        
        Console.WriteLine(); // Додаємо порожній рядок для кращої читабельності
    }

    /// <summary>
    /// Перевіряє, чи всі слова в тексті починаються з великої літери, і виводить список невідповідностей.
    /// </summary>
    /// <param name="inputText">Текст для перевірки.</param>
    public static void CheckWordsCapitalization(string inputText)
    {
        const string description = "Перевірка: чи всі слова починаються з великої літери";
        // \p{Ll} - будь-яка мала літера в Unicode
        const string pattern = @"\b\p{Ll}\p{L}*['-]?\p{L}*\b";

        Console.WriteLine($"--- {description} ---");

        if (string.IsNullOrEmpty(inputText))
        {
            Console.WriteLine("Помилка: Вхідний текст порожній або null.");
            Console.WriteLine();
            return;
        }

        try
        {
            MatchCollection matches = Regex.Matches(inputText, pattern, RegexOptions.Compiled);

            if (matches.Count > 0)
            {
                Console.WriteLine($"Знайдено {matches.Count} слів, що починаються з малої літери (невідповідності):");
                foreach (Match match in matches)
                {
                    Console.WriteLine($"- {match.Value}");
                }
            }
            else
            {
                Console.WriteLine("Успіх! Всі слова в тексті починаються з великої літери.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Під час перевірки сталася помилка: {ex.Message}");
        }

        Console.WriteLine();
    }
}
