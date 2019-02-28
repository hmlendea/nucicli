using System;
using System.Collections.Generic;
using System.Linq;

using NuciCLI;
using NuciCLI.Extensions;

namespace NuciCLI.Menus
{
    /// <summary>
    /// Command-line menu.
    /// </summary>
    public class Menu
    {
        readonly Dictionary<string, Command> commands;

        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets or sets the title colour.
        /// </summary>
        /// <value>The title colour.</value>
        public Colour TitleColour { get; set; }

        /// <summary>
        /// Gets or sets the title decoration colour.
        /// </summary>
        /// <value>The title decoration colour.</value>
        public Colour TitleDecorationColour { get; set; }

        /// <summary>
        /// Gets or sets the prompt colour.
        /// </summary>
        /// <value>The prompt colour.</value>
        public Colour PromptColour { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the title decoration.
        /// </summary>
        /// <value>The title decoration.</value>
        public string TitleDecoration { get; set; } = "-==< ";

        /// <summary>
        /// Gets or sets the prompt.
        /// </summary>
        /// <value>The prompt.</value>
        public string Prompt { get; set; } = "> ";

        public bool AreStatisticsEnabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        public Menu()
        {
            commands = new Dictionary<string, Command>();

            TitleColour = Colour.Green;
            TitleDecorationColour = Colour.Yellow;
            PromptColour = Colour.White;

            AddCommand("exit", "Exit this menu", Exit);
            AddCommand("help", "Prints the command list", PrintCommandList);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="title">Title.</param>
        public Menu(string title)
            : this()
        {
            Title = title;
        }

        /// <summary>
        /// Inputs the permission.
        /// </summary>
        /// <returns><c>true</c>, if permission was input, <c>false</c> otherwise.</returns>
        /// <param name="prompt">Prompt.</param>
        public bool InputPermission(string prompt)
        {
            while (true)
            {
                string inputValue = NuciConsole.ReadLine($"{prompt} (y/N) ");

                if (string.IsNullOrWhiteSpace(inputValue))
                {
                    NuciConsole.WriteLine();
                    return true;
                }
                else if (inputValue.Length > 1)
                {
                    continue;
                }

                switch (inputValue[0])
                {
                    case 'y':
                    case 'Y':
                        NuciConsole.WriteLine();
                        return true;

                    case 'n':
                    case 'N':
                        NuciConsole.WriteLine();
                        return false;

                    default:
                        NuciConsole.CursorX -= 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Adds the command.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="action">The action.</param>
        public void AddCommand(string name, string description, Action action)
        {
            Command command = new Command(name, description, action);
            
            commands.Add(command.Name, command);
        }

        /// <summary>
        /// Runs this menu.
        /// </summary>
        public void Run()
        {
            IsRunning = true;

            PrintTitle();
            PrintCommandList();

            while (IsRunning)
            {
                NuciConsole.WriteLine();

                string cmd = NuciConsole.ReadLine(Prompt, PromptColour);
                
                HandleCommand(cmd);
            }
        }

        /// <summary>
        /// Prints the title.
        /// </summary>
        void PrintTitle()
        {
            NuciConsole.Write(TitleDecoration, TitleDecorationColour);
            NuciConsole.Write(Title, TitleColour);
            NuciConsole.Write(TitleDecoration.Reverse(), TitleDecorationColour);

            NuciConsole.WriteLine();
        }

        /// <summary>
        /// Prints the command list.
        /// </summary>
        void PrintCommandList()
        {
            int commandColumnWidth = commands.Keys.Max(x => x.Length) + 4;

            foreach (Command command in commands.Values)
            {
                string formattedName = command.Name.PadRight(commandColumnWidth);

                NuciConsole.WriteLine($"{formattedName} {command.Description}");
            }
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        void HandleCommand(string cmd)
        {
            if (!commands.ContainsKey(cmd))
            {
                NuciConsole.WriteLine("Unknown command", Colour.Red);
                return;
            }

            CommandResult result = commands[cmd].Execute();

            if (AreStatisticsEnabled)
            {
                PrintCommandResults(result);
            }
        }

        /// <summary>
        /// Exit this menu.
        /// </summary>
        void Exit()
        {
            IsRunning = false;
        }

        void PrintCommandResults(CommandResult result)
        {
            NuciConsole.Write("Command finished ");
            
            if (result.WasSuccessful)
            {
                NuciConsole.Write("sucessfully", Colour.Green);
            }
            else
            {
                NuciConsole.Write("unsucessfully", Colour.Red);
            }
            
            NuciConsole.Write(" in ");

            if (result.Duration.TotalSeconds < 1)
            {
                NuciConsole.WriteLine($"{result.Duration.TotalMilliseconds}ms");
            }
            else if (result.Duration.TotalMinutes < 1)
            {
                NuciConsole.WriteLine($"{result.Duration.TotalSeconds}s");
            }
            else
            {
                NuciConsole.WriteLine($"{result.Duration.TotalMinutes}m");
            }

            if (!result.WasSuccessful)
            {
                NuciConsole.WriteLine($"Error message: {result.Exception.Message}", Colour.Red);
                throw result.Exception;
            }
        }
    }
}
