using System;
using UnityEngine;
using Utilities;

namespace Player
{
    public class BaseModule : MonoBehaviour
    {
        public Module module;
        protected InputControls inputControls;
        private PlayerController _playerController;

        protected virtual void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            inputControls = new InputControls();
        }

        private void OnEnable()
        {
            inputControls.Enable();
        }

        private void OnDisable()
        {
            inputControls.Disable();
        }

        private void Start()
        {
            _playerController.modules.Add(this);
            enabled = false;
        }
    }
}
