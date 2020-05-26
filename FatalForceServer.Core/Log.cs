using System;

namespace FatalForceServer.Core
{
    public class Log
    {
        public static void Info(string text)
        {
            Console.Write($"[{DateTime.UtcNow}] [INFO] {text}");
            Console.WriteLine();
        }

        public static void Error(string text)
        {
            Console.Write($"[{DateTime.UtcNow}] [ERROR] {text}");
            Console.WriteLine();
        }
    }
}
