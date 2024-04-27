using System;
using UnityEngine;
using Utilities;

namespace Player
{
    public class BaseModule : MonoBehaviour
    {
        public Module module;
        protected InputControls inputControls;
        protected PlayerController playerController;

        protected virtual void Awake()
        {
            playerController = GetComponent<PlayerController>();
            inputControls = new InputControls();
        }

        protected virtual void OnEnable()
        {
            inputControls.Enable();
        }

        protected virtual  void OnDisable()
        {
            inputControls.Disable();
        }

        private void Start()
        {
            playerController.modules.Add(this);
            enabled = false;
        }
    }
}
