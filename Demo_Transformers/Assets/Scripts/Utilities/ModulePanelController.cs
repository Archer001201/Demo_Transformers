using System;
using TMPro;
using UnityEngine;

namespace Utilities
{
    public class ModulePanelController : MonoBehaviour
    {
        public TextMeshProUGUI currentText;
        public TextMeshProUGUI clipboardText;
        public GameObject moduleTextPrefab;
        public Transform historyContainer;

        private void OnEnable()
        {
            EventHandler.updateCurrentText += OnUpdateCurrentText;
            EventHandler.updateClipboardText += OnUpdateClipboardText;
            EventHandler.addModuleToHistory += OnAddModuleToHistory;
            EventHandler.removeModuleFromHistory += OnRemoveModuleFromHistory;
        }

        private void OnDisable()
        {
            EventHandler.updateCurrentText -= OnUpdateCurrentText;
            EventHandler.updateClipboardText -= OnUpdateClipboardText;
            EventHandler.addModuleToHistory -= OnAddModuleToHistory;
            EventHandler.removeModuleFromHistory -= OnRemoveModuleFromHistory;
        }

        private void OnUpdateCurrentText(Module module)
        {
            currentText.text = TranslateModuleText(module);
        }

        private void OnUpdateClipboardText(Module module)
        {
            clipboardText.text = TranslateModuleText(module);
        }

        private void OnAddModuleToHistory(Module module)
        {
            var instance = Instantiate(moduleTextPrefab, historyContainer);
            instance.GetComponent<TextMeshProUGUI>().text = TranslateModuleText(module);
        }

        private void OnRemoveModuleFromHistory(int index)
        {
            Destroy(historyContainer.GetChild(index).gameObject);
        }

        private static string TranslateModuleText(Module module)
        {
            var moduleName = module switch
            {
                Module.Firepower => "火力",
                Module.Magnet => "磁力",
                Module.Thruster => "推进器",
                _ => string.Empty
            };
            return moduleName;
        }
    }
}
