using System;
using UnityEngine;

namespace Utilities
{
    public static class EventHandler
    {
        public static Action<bool> ctrlPressed;

        public static void OnCtrlPressed(bool isPressed)
        {
            ctrlPressed?.Invoke(isPressed);
        }

        public static Action<Module> updateCurrentText;

        public static void OnUpdateCurrentText(Module module)
        {
            updateCurrentText?.Invoke(module);
        }
        
        public static Action<Module> updateClipboardText;

        public static void OnUpdateClipboardText(Module module)
        {
            updateClipboardText?.Invoke(module);
        }

        public static Action<Module> addModuleToHistory;

        public static void OnAddModuleToHistory(Module module)
        {
            addModuleToHistory?.Invoke(module);
        }

        public static Action<int> removeModuleFromHistory;

        public static void OnRemoveModuleFromHistory(int index)
        {
            removeModuleFromHistory?.Invoke(index);
        }

        public static Action<Attribute, bool> updateAttributePanel;

        public static void OnUpdateAttributePanel(Attribute attribute, bool isAdding)
        {
            updateAttributePanel?.Invoke(attribute, isAdding);
        }

        public static Action<bool> enableJump;

        public static void OnEnableJump(bool enabled)
        {
            enableJump?.Invoke(enabled);
        }
    }
}
