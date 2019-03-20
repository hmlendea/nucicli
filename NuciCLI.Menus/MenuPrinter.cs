using System;
using System.Collections.Generic;
using System.Linq;

using NuciCLI.Extensions;

namespace NuciCLI.Menus
{
    internal class MenuPrinter
    {
        /// <summary>
        /// Prints the title.
        /// </summary>
        public static void PrintTitle(string title, string decoration, NuciConsoleColour titleColour, NuciConsoleColour decorationColour)
        {
            NuciConsole.Write(decoration, decorationColour);
            NuciConsole.Write(title, titleColour);
            NuciConsole.Write(decoration.Reverse(), decorationColour);

            NuciConsole.WriteLine();
        }

        /// <summary>
        /// Prints the command list.
        /// </summary>
        public static void PrintCommandList(Dictionary<string, Command> commands)
        {
            int commandColumnWidth = commands.Keys.Max(x => x.Length) + 4;

            foreach (Command command in commands.Values)
            {
                string formattedName = command.Name.PadRight(commandColumnWidth);

                NuciConsole.WriteLine($"{formattedName} {command.Description}");
            }
        }

        public static void PrintCommandResults(CommandResult result)
        {
            NuciConsole.WriteLine();
            NuciConsole.Write("Command finished ");

            string durationString = GetHumanFriendlyDurationString(result.Duration);
            
            if (result.WasSuccessful)
            {
                NuciConsole.Write("sucessfully", NuciConsoleColour.Green);
            }
            else
            {
                NuciConsole.Write("unsucessfully", NuciConsoleColour.Red);
            }
            
            NuciConsole.Write($" in {durationString}");

            if (!result.WasSuccessful)
            {
                NuciConsole.WriteLine($"Error message: {result.Exception.Message}", NuciConsoleColour.Red);
                throw result.Exception;
            }
        }

        private static string GetHumanFriendlyDurationString(TimeSpan timeSpan)
        {
            if (timeSpan.TotalSeconds < 1)
            {
                return $"{timeSpan.TotalMilliseconds}ms";
            }
            else if (timeSpan.TotalMinutes < 1)
            {
                return $"{timeSpan.TotalSeconds}s";
            }
            else
            {
                return $"{timeSpan.TotalMinutes}m";
            }
        }
    }
}
