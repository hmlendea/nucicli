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
        public string Id { get; set; }

        public string ParentId { get; set; }

        public IList<string> ChildrenIds { get; set; }

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

        public bool IsDisposed { get; private set; }

        public bool IsRunning { get; private set; }

        /// <summary>
        /// Occurs when this <see cref="Menu"/> was created.
        /// </summary>
        public event EventHandler Created;

        /// <summary>
        /// Occurs when this <see cref="Menu"/> was disposed.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Occurs when this <see cref="Menu"/> was started.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Occurs when this <see cref="Menu"/> was stopped.
        /// </summary>
        public event EventHandler Stopped;
        
        readonly Dictionary<string, Command> commands;

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
            AddCommand("help", "Prints the command list", HandleHelp);

            Created?.Invoke(this, EventArgs.Empty);
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
            if (IsDisposed || !disposing)
            {
                return;
            }

            commands.Clear();
            IsRunning = false;

            IsDisposed = true;

            foreach (string childId in ChildrenIds)
            {
                MenuManager.Instance.CloseMenu(childId);
            }
            
            MenuManager.Instance.CloseMenu(Id);
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public void Start()
        {
            IsRunning = true;

            MenuPrinter.PrintTitle(Title, TitleDecoration, TitleColour, TitleDecorationColour);
            MenuPrinter.PrintCommandList(commands);

            Started?.Invoke(this, EventArgs.Empty);

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

            Stopped?.Invoke(this, EventArgs.Empty);
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
                MenuPrinter.PrintCommandResults(result);
            }

            NuciConsole.WriteLine();
        }

        void HandleHelp()
        {
            MenuPrinter.PrintCommandList(commands);
        }
    }
}
