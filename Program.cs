using SocialMediaPostManager;
using SocialMediaPostManager.Menus;

Startup.CreateTables();
AppMenu appMenu = new AppMenu();
appMenu.StartApp();