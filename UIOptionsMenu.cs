using Kitchen;
using KitchenLib.DevUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdvancedUIReboot
{
    internal class UIOptionsMenu : BaseUI
    {
        public UIOptionsMenu() 
        {
            ButtonName = "AdvancedUI";
        }

        public override void Setup()
        {
            GUILayout.Label("Advanced UI Options");

            if (GameInfo.CurrentScene == SceneType.Kitchen)
            {
                GUILayout.Button()
            }
        }
    }
}
