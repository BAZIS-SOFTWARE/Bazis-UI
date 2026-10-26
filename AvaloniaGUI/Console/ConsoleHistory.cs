using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaGUI.Console
{
    public static class ConsoleHistory
    {
        private const int MaxHistory = 9;
        private static List<string> history = new List<string>();
        private static int index = 0;

        public static void AddComand(string command)
        {
            if (history.LastOrDefault() == command)
            {
                // Повтор последней команды (например, выбранной стрелками) тоже возвращает навигацию в конец истории.
                index = history.Count;
                return;
            }

            history.Add(command);

            if (history.Count > MaxHistory)
                history.RemoveAt(0);

            index = history.Count;
        }

        public static string GetPreviousCommand()
        {
            if (history.Count == 0)
                return string.Empty;
            index = Math.Max(0, index - 1);
            return history[index];
        }

        public static string GetNextCommand()
        {
            if (history.Count == 0)
                return string.Empty;
            index = Math.Min(history.Count, index + 1);

            if (index == history.Count)
                return string.Empty; // пустая строка для новой команды

            return history[index];
        }
    }
}
