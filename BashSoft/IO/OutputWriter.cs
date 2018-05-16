namespace BashSoft
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class OutputWriter
    {
        public static void WriteMessage(string message)
        {
            Console.Write(message);
        }
        public static void WriteMessageOnNewLine(string message)
        {
            Console.WriteLine(message);
        }
        public static void WriteEmptyLine()
        {
            Console.WriteLine();
        }
        public static void DisplayException(string message)
        {
            var currantForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = currantForegroundColor;
        }

        public static void PrintStudent(KeyValuePair<string, double> student)
        {
            OutputWriter.WriteMessageOnNewLine($"{student.Key} - {string.Join(", ", student.Value)}");
        }

        public static void DisplayException(object comparisonOfFilesWithDifferentSizes)
        {
            throw new NotImplementedException();
        }
    }
}
