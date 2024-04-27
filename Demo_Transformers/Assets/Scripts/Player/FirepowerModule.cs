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
            inputControls.Gameplay.LeftMousePressed.performed += StartFire;
            inputControls.Gameplay.LeftMousePressed.canceled += StopFire;
        }

        private void Fire(InputAction.CallbackContext context)
        {
            var projectile = Instantiate(projectilePrefab, firePosition.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().AddForce(firePosition.forward * 100f, ForceMode.Impulse); 
        }

        private IEnumerator FireCoroutine()
        {
            while (true)
            {
                var projectile = Instantiate(projectilePrefab, firePosition.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody>().AddForce(firePosition.forward * 100f, ForceMode.Impulse);
                yield return new WaitForSeconds(0.5f);  
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private void StartFire(InputAction.CallbackContext context)
        {
            if (_fireCoroutine != null) return;
            _fireCoroutine = StartCoroutine(FireCoroutine());
        }

        private void StopFire(InputAction.CallbackContext context)
        {
            if (_fireCoroutine == null) return;
            StopCoroutine(_fireCoroutine);
            _fireCoroutine = null;
        }
    }
}
