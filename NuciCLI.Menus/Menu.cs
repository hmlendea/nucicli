using System;
using System.Collections.Generic;
using System.Linq;

using NuciCLI.Extensions;

namespace NuciCLI.Menus
{
    /// <summary>
    /// Command-line menu.
    /// </summary>
    public class Menu : IDisposable
    {
        readonly Dictionary<string, Command> commands;

        public string Id { get; set; }

        public string ParentId { get; set; }

        public IList<string> ChildrenIds { get; set; }

        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets or sets the title colour.
        /// </summary>
        /// <value>The title colour.</value>
        public NuciConsoleColour TitleColour { get; set; }

        /// <summary>
        /// Gets or sets the title decoration colour.
        /// </summary>
        /// <value>The title decoration colour.</value>
        public NuciConsoleColour TitleDecorationColour { get; set; }

        /// <summary>
        /// Gets or sets the prompt colour.
        /// </summary>
        /// <value>The prompt colour.</value>
        public NuciConsoleColour PromptColour { get; set; }

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

        bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        public Menu()
        {
            commands = new Dictionary<string, Command>();
            
            Id = Guid.NewGuid().ToString();
            ChildrenIds = new List<string>();

            TitleColour = NuciConsoleColour.Green;
            TitleDecorationColour = NuciConsoleColour.Yellow;
            PromptColour = NuciConsoleColour.White;

            AddCommand("exit", "Exit this menu", Dispose);
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

        ~Menu()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed || !disposing)
            {
                return;
            }

            commands.Clear();
            IsRunning = false;

            disposed = true;

            foreach (string childId in ChildrenIds)
            {
                MenuManager.Instance.CloseMenu(childId);
            }
            
            MenuManager.Instance.CloseMenu(Id);
        }

        public void Start()
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

        public void Stop()
        {
            IsRunning = false;
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
                NuciConsole.WriteLine("Unknown command", NuciConsoleColour.Red);
                return;
            }

            CommandResult result = commands[cmd].Execute();

            if (AreStatisticsEnabled)
            {
                PrintCommandResults(result);
            }

            NuciConsole.WriteLine();
        }

        void PrintCommandResults(CommandResult result)
        {
            NuciConsole.WriteLine();
            NuciConsole.Write("Command finished ");
            
            if (result.WasSuccessful)
            {
                NuciConsole.Write("sucessfully", NuciConsoleColour.Green);
            }
            else
            {
                NuciConsole.Write("unsucessfully", NuciConsoleColour.Red);
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
                NuciConsole.WriteLine($"Error message: {result.Exception.Message}", NuciConsoleColour.Red);
                throw result.Exception;
            }
        }
    }
}
