using McMaster.Extensions.CommandLineUtils;
using System;

namespace YoCli.Commons
{
    static class ConsoleMessage
    {
        public static void PrintError(IConsole console, string message)
        {
            console.ForegroundColor = ConsoleColor.DarkRed;
            console.WriteLine(message);
            console.ResetColor();
        }

        public static void PrintSuccess(IConsole console, string message)
        {
            console.ForegroundColor = ConsoleColor.Green;
            console.WriteLine(message);
            console.ResetColor();
        }
    }
}
