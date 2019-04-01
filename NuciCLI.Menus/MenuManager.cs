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
        static volatile MenuManager instance;
        static object syncRoot = new object();

        IDictionary<string, Menu> menus;

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

                curMenu.Stop();

                newMenu.ParentId = curMenu.Id;
                NuciConsole.WriteLine();
            }

            CurrentMenuId = newMenu.Id;
            newMenu.Start();
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

            menu.Dispose();
            menus.Remove(menuId);

            if (!string.IsNullOrWhiteSpace(parentId))
            {
                NuciConsole.WriteLine();

                CurrentMenuId = parentId;
                menus[CurrentMenuId].Start();
            }
        }
    }
}
