using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AdvancedUIReboot
{
    public class MenuController
    {
        public Dictionary<UIElement, MenuBehavior> MenuBehaviourDictionary;
        public ModPreference Preferences;

        private static string _path = Path.Combine(Application.persistentDataPath, "Progress", "AdvancedUI.pref");

        public MenuController()
        {
            MenuBehaviourDictionary = new Dictionary<UIElement, MenuBehavior>
            {
                { UIElement.Coin, new MenuBehavior() },
                { UIElement.Group, new MenuBehavior() },
                { UIElement.Menu, new MenuBehavior() },
                { UIElement.Time, new MenuBehavior() },
                { UIElement.Day, new MenuBehavior() },
                { UIElement.PlayerList, new MenuBehavior() },
                { UIElement.VersionInfo, new MenuBehavior() }
            };

            if (File.Exists(_path))
            {
                Preferences = JsonConvert.DeserializeObject<ModPreference>(File.ReadAllText(_path));
            }
            else
            {
                Preferences = new ModPreference();
            }
        }

        public Vector3? GetPreferenceValue(UIElement element)
        {
            return element switch
            {
                UIElement.Coin => Mod.Controller.Preferences.CoinDisplayPos,
                UIElement.Group => Mod.Controller.Preferences.GroupDisplayPos,
                UIElement.Menu => Mod.Controller.Preferences.MenuDisplayPos,
                UIElement.Time => Mod.Controller.Preferences.TimeDisplayPos,
                UIElement.Day => Mod.Controller.Preferences.DayDisplayPos,
                UIElement.PlayerList => Mod.Controller.Preferences.PlayerListDisplayPos,
                UIElement.VersionInfo => Mod.Controller.Preferences.VersionInfoDisplayPos
            };
        }

        public void SetPreferenceValue(UIElement element, Vector3 pos)
        {
            switch (element)
            {
                case UIElement.Coin:
                    Mod.Controller.Preferences.CoinDisplayPos = pos;
                    break;
                case UIElement.Group:
                    Mod.Controller.Preferences.GroupDisplayPos = pos;
                    break;
                case UIElement.Menu:
                    Mod.Controller.Preferences.MenuDisplayPos = pos;
                    break;
                case UIElement.Time: 
                    Mod.Controller.Preferences.TimeDisplayPos = pos;
                    break;
                case UIElement.Day:
                    Mod.Controller.Preferences.DayDisplayPos = pos;
                    break;
                case UIElement.PlayerList:
                    Mod.Controller.Preferences.PlayerListDisplayPos = pos;
                    break;
                 case UIElement.VersionInfo:
                    Mod.Controller.Preferences.VersionInfoDisplayPos = pos;
                    break;
            }
        }

        public bool PreferenceHasValue(UIElement element)
        {
            return element switch
            {
                UIElement.Coin => Mod.Controller.Preferences.CoinDisplayPos.HasValue,
                UIElement.Group => Mod.Controller.Preferences.GroupDisplayPos.HasValue,
                UIElement.Menu => Mod.Controller.Preferences.MenuDisplayPos.HasValue,
                UIElement.Time => Mod.Controller.Preferences.TimeDisplayPos.HasValue,
                UIElement.Day => Mod.Controller.Preferences.DayDisplayPos.HasValue,
                UIElement.PlayerList => Mod.Controller.Preferences.PlayerListDisplayPos.HasValue,
                UIElement.VersionInfo => Mod.Controller.Preferences.VersionInfoDisplayPos.HasValue
            };
        }

        public void SavePreference()
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(Preferences, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        public void ResetPreference()
        {
            foreach (UIElement element in PreferenceSystemManagerExtensions._elements)
            {
                if (Mod.Controller.MenuBehaviourDictionary.TryGetValue(element, out MenuBehavior behavior))
                {
                    if (behavior.initialized)
                    {
                        behavior.visibility_changed = true;
                        SetPreferenceValue(element, behavior.InitialPos.Value);
                    }
                }
            }
        }

        public bool GetElementPosition(UIElement element)
        {
            return Mod.PrefManager.Get<bool>($"{element} Position");
        }

        public bool GetElementVisibility(UIElement element)
        {
            return Mod.PrefManager.Get<bool>($"{element} Visibility");
        }
    }
}