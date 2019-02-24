using System;
using System.Collections.Generic;
using System.Linq;

namespace NuciCLI.Menus
{
    /// <summary>
    /// Command-line menu.
    /// </summary>
    public class Menu
    {
        string cmd;
        bool isRunning;

        readonly Dictionary<string, Command> commands;

        /// <summary>
        /// Gets or sets the title colour.
        /// </summary>
        /// <value>The title colour.</value>
        public ConsoleColor TitleColour { get; set; }

        /// <summary>
        /// Gets or sets the title decoration colour.
        /// </summary>
        /// <value>The title decoration colour.</value>
        public ConsoleColor TitleDecorationColour { get; set; }

        /// <summary>
        /// Gets or sets the prompt colour.
        /// </summary>
        /// <value>The prompt colour.</value>
        public ConsoleColor PromptColour { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the title decoration on the left.
        /// </summary>
        /// <value>The title decoration on the left.</value>
        public string TitleDecorationLeft { get; set; } = "-==< ";

        /// <summary>
        /// Gets or sets the title decoration on the right.
        /// </summary>
        /// <value>The title decoration on the right.</value>
        public string TitleDecorationRight { get; set; } = " >==-";

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

            TitleColour = ConsoleColor.Green;
            TitleDecorationColour = ConsoleColor.Yellow;
            PromptColour = ConsoleColor.White;

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
        /// Input the specified prompt.
        /// </summary>
        /// <param name="prompt">Prompt.</param>
        public virtual string Input(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        /// <summary>
        /// Inputs the permission.
        /// </summary>
        /// <returns><c>true</c>, if permission was input, <c>false</c> otherwise.</returns>
        /// <param name="prompt">Prompt.</param>
        public bool InputPermission(string prompt)
        {
            Console.Write(prompt);
            Console.Write(" (y/N) ");

            while (true)
            {
                ConsoleKeyInfo c = Console.ReadKey();

                switch (c.Key)
                {
                    case ConsoleKey.Y:
                        Console.WriteLine();
                        return true;

                    case ConsoleKey.N:
                    case ConsoleKey.Enter:
                        Console.WriteLine();
                        return false;

                    default:
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
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
            isRunning = true;

            PrintTitle();
            PrintCommandList();

            while (isRunning)
            {
                Console.WriteLine();

                GetCommand();
                HandleCommand();
            }
        }

        /// <summary>
        /// Prints the title.
        /// </summary>
        void PrintTitle()
        {
            ConsoleEx.WriteColoured(TitleDecorationLeft, TitleDecorationColour);
            ConsoleEx.WriteColoured(Title, TitleColour);
            ConsoleEx.WriteColoured(TitleDecorationRight, TitleDecorationColour);
            Console.ResetColor(); // TODO: Proper colour fix

            Console.WriteLine();
        }

        /// <summary>
        /// Prints the command list.
        /// </summary>
        void PrintCommandList()
        {
            int commandColumnWidth = commands.Keys
                .Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur)
                .Length + 4;

            foreach (Command command in commands.Values)
            {
                Console.WriteLine("{0} {1}", command.Name.PadRight(commandColumnWidth), command.Description);
            }
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <returns>The command.</returns>
        string GetCommand()
        {
            ConsoleEx.WriteColoured(Prompt, PromptColour);

            cmd = Console.ReadLine();
            return cmd;
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        void HandleCommand()
        {
            if (!commands.ContainsKey(cmd))
            {
                Console.WriteLine("Invalid command");
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
            isRunning = false;
        }

        void PrintCommandResults(CommandResult result)
        {
            Console.Write("Command finished ");
            
            if (result.WasSuccessful)
            {
                ConsoleEx.WriteColoured("sucessfully", ConsoleColor.Green);
            }
            else
            {
                ConsoleEx.WriteColoured("unsucessfully", ConsoleColor.Red);
            }
            
            Console.Write(" in ");

            if (result.Duration.TotalSeconds < 1)
            {
                Console.WriteLine($"{result.Duration.TotalMilliseconds}ms");
            }
            else if (result.Duration.TotalMinutes < 1)
            {
                Console.WriteLine($"{result.Duration.TotalSeconds}s");
            }
            else
            {
                Console.WriteLine($"{result.Duration.TotalMinutes}m");
            }

            if (!result.WasSuccessful)
            {
                Console.WriteLine($"Error message: {result.Exception.Message}");
                throw result.Exception;
            }
        }
    }
}
