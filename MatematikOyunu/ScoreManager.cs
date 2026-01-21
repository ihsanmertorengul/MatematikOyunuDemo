using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MatematikOyunu
{
    public static class ScoreManager
    {
        private static string filePath = "scores.txt";

        // Süre ekle
        public static void AddScore(TimeSpan time)
        {
            File.AppendAllText(filePath, time.TotalSeconds + Environment.NewLine);
        }

        // En iyi süreleri getir
        public static List<TimeSpan> GetBestScores(int count = 5)
        {
            if (!File.Exists(filePath))
                return new List<TimeSpan>();

            return File.ReadAllLines(filePath)
                .Select(x => TimeSpan.FromSeconds(double.Parse(x)))
                .OrderBy(x => x) // ⏱ kısa süre = iyi
                .Take(count)
                .ToList();
        }
    }
}
