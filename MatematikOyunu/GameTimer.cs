using System;
using System.Diagnostics;

namespace MatematikOyunu
{
    public static class GameTimer
    {
        private static Stopwatch stopwatch = new Stopwatch();

        public static void Start()
        {
            stopwatch.Reset();
            stopwatch.Start();
        }

        public static void Stop()
        {
            stopwatch.Stop();
        }

        public static TimeSpan GetTime()
        {
            return stopwatch.Elapsed;
        }

        public static string GetFormattedTime()
        {
            TimeSpan t = stopwatch.Elapsed;
            return $"{t.Minutes:D2}:{t.Seconds:D2}";
        }
    }
}
