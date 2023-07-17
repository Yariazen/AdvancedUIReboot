using UnityEngine;

namespace AdvancedUIReboot
{
    public class MenuBehavior
    {
        public bool initialized = false;

        public Vector3? InitialPos = null;

        public bool draggable = false;
        public bool dragging = false;

        public bool visibility = false;
        public bool visibility_changed = false;
    }
}
