using PreferenceSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedUIReboot
{
    public static class PreferenceSystemManagerExtensions
    {
        internal static IEnumerable<UIElement> _elements = Enum
            .GetValues(typeof(UIElement))
            .Cast<UIElement>();

        private static Action<bool> GetPositionOnActivate(UIElement element)
        {
            return (selectedIndex) =>
            {
                Mod.Controller.MenuBehaviourDictionary[element].draggable = selectedIndex;
                Mod.Controller.SavePreference();
            };
        }

        public static PreferenceSystemManager FillElementPosition(this PreferenceSystemManager PrefManager)
        {
            return _elements
                .Aggregate(PrefManager, (manager, element) => manager
                    .AddLabel($"{element} Position")
                    .AddOption($"{element} Position", false, new bool[] { false, true }, new string[] { "Default", "Draggable" }, GetPositionOnActivate(element))
                );
        }

        private static Action<bool> GetVisibilityOnActivate(UIElement element)
        {
            return (selectedIndex) =>
            {
                Mod.Controller.MenuBehaviourDictionary[element].visibility = selectedIndex;
                Mod.Controller.MenuBehaviourDictionary[element].visibility_changed = true;
                Mod.Controller.SavePreference();
            };
        }

        public static PreferenceSystemManager FillElementVisibility(this PreferenceSystemManager PrefManager)
        {
            return _elements
                .Aggregate(PrefManager, (manager, element) => manager
                    .AddLabel($"{element} Visibility")
                    .AddOption($"{element} Visibility", false, new bool[] { false, true }, new string[] { "Shown", "Hidden" }, GetVisibilityOnActivate(element))
                );
        }

        private static bool isNotAdjustible(UIElement element)
        {
            if (Mod.Controller.MenuBehaviourDictionary.TryGetValue(element, out MenuBehavior behavior))
                return !(behavior.draggable && behavior.InitialPos.HasValue);
            return true;
        }

        private static Action<int> GetAdjustPositionOnActivate(UIElement element)
        {
            return (selectedIndex) =>
            {
                Mod.Controller.MenuBehaviourDictionary[element].dragging = true;
            };
        }

        public static PreferenceSystemManager FillAdjustPosition(this PreferenceSystemManager PrefManager)
        {
            return _elements
                .Aggregate(PrefManager, (manager, element) => manager
                    .AddConditionalBlocker(() => isNotAdjustible(element))
                        .AddButton($"{element} Element", GetAdjustPositionOnActivate(element))
                    .ConditionalBlockerDone()
                );
        }
    }
}
