using KitchenMods;
using PreferenceSystem;

namespace AdvancedUIReboot
{
    public class Mod : IModInitializer
    {
        public const string ModVersion = "1.1.0";
        public const string ModName = "Advanced UI Reboot";
        public const string ModAuthor = "Yariazen";

        internal static PreferenceSystemManager PrefManager;
        internal static MenuController Controller;

        public void PostActivate(KitchenMods.Mod mod)
        {
            Controller = new MenuController();

            PrefManager = new PreferenceSystemManager($"{ModAuthor}.{ModName}", ModName);
            PrefManager
                .AddLabel("Advanced UI Reboot")
                .AddSubmenu("Element Position", "element_position")
                    .AddLabel("Element Position")
                    .FillElementPosition()
                .SubmenuDone()
                .AddSubmenu("Element Visibility", "element_visibility")
                    .AddLabel("Element Visibility")
                    .FillElementVisibility()
                .SubmenuDone()
                .AddSubmenu("Adjust Position", "adjust_position")
                    .AddLabel("Adjust Position")
                    .FillAdjustPosition()
                .SubmenuDone()
                .AddButton("Reset", (selectedIndex) =>
                {
                    Controller.ResetPreference();
                });

            PrefManager.RegisterMenu(PreferenceSystemManager.MenuType.MainMenu);
            PrefManager.RegisterMenu(PreferenceSystemManager.MenuType.PauseMenu);
        }

        public void PostInject() { }
        public void PreInject() { }
    }
}
