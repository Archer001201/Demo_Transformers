using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using Module = Utilities.Module;

namespace Player
{
    public class MagnetModule : BaseModule
    {
        public Transform rayStartPosition;
        public float rayDetectionRange;
        public GameObject target;
        
        protected override void Awake()
        {
            base.Awake();
            module = Module.Magnet;
            inputControls.Gameplay.LeftMousePressed.performed += GrabTargetItem;
        }

        private void Update()
        {
            var ray = new Ray(rayStartPosition.position, rayStartPosition.forward);
            if (Physics.Raycast(ray, out var hit, rayDetectionRange))
            {
                var detectedObj = hit.collider.gameObject;
                if (detectedObj.CompareTag("MagneticItem"))
                {
                    target = detectedObj;
                }
                else target = null;
            }
            else target = null;
            
            Debug.DrawRay(ray.origin, ray.direction * rayDetectionRange, Color.blue);
        }

        private void GrabTargetItem(InputAction.CallbackContext context)
        {
            if (target == null) return;
            target.transform.position = rayStartPosition.position;
            Debug.Log("Grab");
        }
    }
}
