using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NuciCLI.Menus
{
    /// <summary>
    /// Menu manager.
    /// </summary>
    public class MenuManager
    {
        public bool IsRunning { get; private set; }

        public bool AreStatisticsEnabled { get; set; }

        static volatile MenuManager instance;
        static object syncRoot = new object();

        IDictionary<string, Menu> menus;

        Menu CurrentMenu => menus[CurrentMenuId];

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static MenuManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new MenuManager();
                        }
                    }
                }

                return instance;
            }
        }

        public string CurrentMenuId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuManager"/> class.
        /// </summary>
        public MenuManager()
        {
            menus = new Dictionary<string, Menu>();
        }

        public void Start<TMenu>() where TMenu : Menu
            => Start<TMenu>(null);

        public void Start<TMenu>(params object[] parameters) where TMenu : Menu
        {
            OpenMenu<TMenu>(parameters);
            Menu menu = menus[CurrentMenuId];

            IsRunning = true;

            while (IsRunning)
            {
                NuciConsole.WriteLine();

                string cmd = NuciConsole.ReadLine(CurrentMenu.Prompt, CurrentMenu.PromptColour);
                
                HandleCommand(cmd);
            }
        }

        public void OpenMenu<TMenu>() where TMenu : Menu
            => OpenMenu<TMenu>(null);

        public void OpenMenu<TMenu>(params object[] parameters) where TMenu : Menu
            => OpenMenu(typeof(TMenu), parameters);

        /// <summary>
        /// Opens the menu.
        /// </summary>
        /// <param name="menuType">Menu type.</param>
        public void OpenMenu(Type menuType)
            => OpenMenu(menuType, null);

        /// <summary>
        /// Opens the menu.
        /// </summary>
        /// <param name="menuType">Menu type.</param>
        /// <param name="parameters">Menu parameters.</param>
        public void OpenMenu(Type menuType, params object[] parameters)
        {   
            Menu newMenu = (Menu)Activator.CreateInstance(menuType, parameters);

            menus.Add(newMenu.Id, newMenu);

            if (!string.IsNullOrWhiteSpace(CurrentMenuId))
            {
                Menu curMenu = menus[CurrentMenuId];

                newMenu.ParentId = curMenu.Id;
                NuciConsole.WriteLine();
            }

            SwitchToMenu(newMenu.Id);
        }

        /// <summary>
        /// Closes the current menu.
        /// </summary>
        public void CloseMenu()
            => CloseMenu(CurrentMenuId);

        public void CloseMenu(string menuId)
        {
            Menu menu = menus[menuId];

            string parentId = menu.ParentId;

            menus.Remove(menuId);
            menu.Dispose();

            if (!string.IsNullOrEmpty(parentId))
            {
                SwitchToMenu(parentId);
            }
            else
            {
                IsRunning = false;
            }
        }

        public void SwitchToMenu(string menuId)
        {
            if (CurrentMenuId == menuId)
            {
                return;
            }
            
            CurrentMenuId = menuId;
            MenuPrinter.PrintMenuHeader(CurrentMenu);
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        void HandleCommand(string cmd)
        {
            if (!CurrentMenu.Commands.ContainsKey(cmd))
            {
                NuciConsole.WriteLine("Unknown command", NuciConsoleColour.Red);
                return;
            }

            Command command = CurrentMenu.Commands[cmd];
            CommandResult result = command.Execute();

            if (AreStatisticsEnabled)
            {
                MenuPrinter.PrintCommandResults(result);
            }

            NuciConsole.WriteLine();
        }
    }
}
