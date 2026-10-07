using System;

namespace ritaripeli
{
    internal class Print
    {
        public static void Line(string text)
        {
            Console.WriteLine(text);
        }

        public static void Write(string text)
        {
            Console.Write(text);
        }

        public static void LineColor(string text, ConsoleColor color)
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = c;
        }

        public static void WriteColor(string text, ConsoleColor color)
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = c;
        }
    }
}