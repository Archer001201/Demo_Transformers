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
        public Camera mainCamera;
        
        protected override void Awake()
        {
            base.Awake();
            module = Module.Firepower;
            inputControls.Gameplay.LeftMousePressed.performed += StartFire;
            inputControls.Gameplay.LeftMousePressed.canceled += StopFire;
            mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }

        private void Fire(InputAction.CallbackContext context)
        {
            var projectile = Instantiate(projectilePrefab, firePosition.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().AddForce(firePosition.forward * 100f, ForceMode.Impulse); 
        }

        private IEnumerator FireCoroutine()
        {
            // while (true)
            // {
            //     var projectile = Instantiate(projectilePrefab, firePosition.position, Quaternion.identity);
            //     projectile.GetComponent<Rigidbody>().AddForce(firePosition.forward * 100f, ForceMode.Impulse);
            //     yield return new WaitForSeconds(0.5f);  
            // }
            while (true)
            {
                // 创建射线从摄像机中心指向屏幕中心
                var ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

                Vector3 targetDirection;
                if (Physics.Raycast(ray, out var hit))
                {
                    // 如果射线碰到了物体，子弹将指向碰撞点
                    targetDirection = (hit.point - firePosition.position).normalized;
                }
                else
                {
                    // 如果射线没有碰到物体，使用射线的方向
                    targetDirection = ray.direction;
                }

                // 发射子弹
                var projectile = Instantiate(projectilePrefab, firePosition.position, Quaternion.identity);
                projectile.transform.forward = targetDirection;
                projectile.GetComponent<Rigidbody>().AddForce(targetDirection * 100f, ForceMode.Impulse);

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
