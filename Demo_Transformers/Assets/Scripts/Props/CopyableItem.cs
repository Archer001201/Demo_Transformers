using System;
using TMPro;
using UnityEngine;
using Utilities;
using EventHandler = Utilities.EventHandler;

namespace Props
{
    public class CopyableItem : MonoBehaviour
    {
        public Module module;
        public TextMeshProUGUI displayName;
        public GameObject hudCanvas;

        private Transform _cameraTrans;

        private void Awake()
        {
            hudCanvas.SetActive(false);
            _cameraTrans = GameObject.FindWithTag("MainCamera").transform;
        }

        private void OnEnable()
        {
            EventHandler.ctrlPressed += OnCtrlPressed;
        }

        private void OnDisable()
        {
            EventHandler.ctrlPressed -= OnCtrlPressed;
        }

        private void Start()
        {
            LoadModuleName();
        }

        private void Update()
        {
            if (hudCanvas.activeSelf)
            {
                hudCanvas.transform.LookAt(_cameraTrans);
                hudCanvas.transform.Rotate(0,180,0);
            }
        }

        private void LoadModuleName()
        {
            var moduleName = module switch
            {
                Module.Firepower => "火力",
                Module.Magnet => "磁力",
                Module.Thruster => "推进器",
                _ => string.Empty
            };
            displayName.text = moduleName;
        }

        private void OnCtrlPressed(bool isPressed)
        {
            hudCanvas.SetActive(isPressed);
        }
    }
}
