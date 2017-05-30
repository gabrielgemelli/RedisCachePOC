using System;
using static System.Console;

namespace CachePOC
{
    static class ConsoleHelper
    {
        static ConsoleColor _originalColor = ForegroundColor;

        public static void WriteErrorMessage(string message, params object[] args)
        {
            WriteErrorMessage(() => WriteLine(message, args));
        }

        public static void WriteSuccessMessage(string message, params object[] args)
        {
            WriteSuccessMessage(() => WriteLine(message, args));
        }

        public static void WriteErrorMessage(string message)
        {
            WriteErrorMessage(() => WriteLine(message));
        }

        public static void WriteSuccessMessage(string message)
        {
            WriteSuccessMessage(() => WriteLine(message));
        }

        private static void WriteErrorMessage(Action writeLine)
        {
            ForegroundColor = ConsoleColor.Red;
            writeLine();
            ForegroundColor = _originalColor;
        }

        private static void WriteSuccessMessage(Action writeLine)
        {
            ForegroundColor = ConsoleColor.Green;
            writeLine();
            ForegroundColor = _originalColor;
        }
    }
}
