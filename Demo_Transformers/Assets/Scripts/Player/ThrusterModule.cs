using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    public class ThrusterModule : BaseModule
    {
        public bool isGliding;
        public float thrusterJumpForce;
        public float fallingSpeed;
        private Rigidbody _rb;
        private float _originalJumpForce;
        
        protected override void Awake()
        {
            base.Awake();
            module = Module.Thruster;
            _rb = GetComponent<Rigidbody>();
            _originalJumpForce = playerController.jumpForce;
            inputControls.Gameplay.Jump.performed += context =>
            {
                if (playerController.isGrounded) return;
                isGliding = !isGliding;
            };
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            playerController.jumpForce = thrusterJumpForce;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            playerController.jumpForce = _originalJumpForce;
        }

        private void Update()
        {
            if (isGliding)
            {
                var glidingSpeed = Mathf.Clamp(_rb.velocity.y, fallingSpeed, 0);
                _rb.velocity = new Vector3(_rb.velocity.x, glidingSpeed, _rb.velocity.z);
            }

            if (playerController.isGrounded) isGliding = false;
        }
    }
}
