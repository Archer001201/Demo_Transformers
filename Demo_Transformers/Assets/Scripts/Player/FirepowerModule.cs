using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    public class FirepowerModule : BaseModule
    {
        public GameObject projectilePrefab;
        public Transform firePosition;
        private Coroutine _fireCoroutine;
        
        protected override void Awake()
        {
            base.Awake();
            module = Module.Firepower;
            inputControls.Gameplay.LeftMousePressed.performed += Fire;
        }

        private void Fire(InputAction.CallbackContext context)
        {
            var projectile = Instantiate(projectilePrefab, firePosition.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().AddForce(firePosition.forward * 100f, ForceMode.Impulse); 
        }
    }
}
