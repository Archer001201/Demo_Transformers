using System;
using DataSO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Props
{
    public class OptionalPanel : MonoBehaviour
    {
        public GameObject optionalPanel;
        public bool isPanelActivated;
        private InputControls _inputControls;

        private void Awake()
        {
            Time.timeScale = 1;
            optionalPanel.SetActive(false);
            _inputControls = new InputControls();
            _inputControls.Gameplay.Esc.performed += _=> OnEscPressed();
        }

        private void OnEnable()
        {
            _inputControls.Enable();
        }

        private void OnDisable()
        {
            _inputControls.Disable();
        }

        public void OnEscPressed()
        {
            isPanelActivated = !isPanelActivated;

            if (isPanelActivated)
            {
                Time.timeScale = 0;
                optionalPanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1;
                optionalPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
