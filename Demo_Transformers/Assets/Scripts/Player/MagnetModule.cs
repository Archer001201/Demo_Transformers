using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using Module = Utilities.Module;

namespace Player
{
    public class MagnetModule : BaseModule
    {
        public Transform rayStartPosition;
        public Transform grabPosition;
        public float rayDetectionRange;
        public GameObject target;
        private Coroutine _grabCoroutine;
        private GameObject _lockedTarget;
        
        protected override void Awake()
        {
            base.Awake();
            module = Module.Magnet;
            inputControls.Gameplay.LeftMousePressed.performed += StartGrab;
            inputControls.Gameplay.LeftMousePressed.canceled += StopGrab;
        }

        private void Update()
        {
            if (_lockedTarget == null)
            {
                var ray = new Ray(rayStartPosition.position, rayStartPosition.forward);
                if (Physics.Raycast(ray, out var hit, rayDetectionRange))
                {
                    var detectedObj = hit.collider.gameObject;
                    if (detectedObj.CompareTag("MagneticCube") || detectedObj.CompareTag("MagneticSphere"))
                    {
                        target = detectedObj;
                    }
                    else target = null;
                }
                else target = null;
                Debug.DrawRay(ray.origin, ray.direction * rayDetectionRange, Color.blue);
            }
        }

        private IEnumerator GrabTargetItem()
        {
            // if (target == null) yield return null;
            // target.transform.position = rayStartPosition.position;
            // Debug.Log("Grab");
            // while (target != null)
            // {
            //     target.transform.position = rayStartPosition.position;
            //     yield return null;
            // }
            if (target == null) yield break; // 没有目标时退出
            _lockedTarget = target; // 锁定目标
        
            while (true) // 持续抓取
            {
                if (_lockedTarget != null)
                {
                    // _lockedTarget.transform.position = rayStartPosition.position;
                    _lockedTarget.transform.position = Vector3.MoveTowards(
                        _lockedTarget.transform.position,
                        grabPosition.position,
                        10f * Time.fixedDeltaTime // 按照速率移动
                    );
                }
                else
                {
                    yield break; // 如果锁定的目标消失，则退出
                }
                yield return null;
            }
        }

        private void StartGrab(InputAction.CallbackContext context)
        {
            if (_grabCoroutine != null) return;
            _grabCoroutine = StartCoroutine(GrabTargetItem());
            _lockedTarget.GetComponent<Rigidbody>().useGravity = false;
        }

        private void StopGrab(InputAction.CallbackContext context)
        {
            if (_grabCoroutine == null) return;
            StopCoroutine(_grabCoroutine);
            _lockedTarget.GetComponent<Rigidbody>().useGravity = true;
            _grabCoroutine = null;
            _lockedTarget = null;
        }
    }
}
