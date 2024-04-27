using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    public class ThrusterModule : BaseModule
    {
        public float liftSpeed = 5f; // 推进器上升速度
        public bool isThrusting;
        public float hoverDuration = 3f; 
        private float _hoverTimer;
        private Coroutine _thrusterCoroutine; // 用于推进器的协程
        private Rigidbody _rb; // 刚体组件
        
        protected override void Awake()
        {
            base.Awake();
            module = Module.Thruster;
            _rb = GetComponent<Rigidbody>();
            inputControls.Gameplay.Thrust.performed += StartThrust;
            inputControls.Gameplay.Thrust.canceled += StopThrust;
        }

        private IEnumerator ThrustCoroutine()
        {
            _hoverTimer = hoverDuration; // 初始化计时器
            while (_hoverTimer > 0) // 当计时器大于0时
            {
                _rb.AddForce(Vector3.up * liftSpeed, ForceMode.Acceleration); // 推进
                _hoverTimer -= Time.deltaTime; // 倒计时
                yield return null; // 等待下一帧
            }

            // 当计时器到达0后，缓慢下降
            while (true)
            {
                float fallingSpeed = Mathf.Clamp(_rb.velocity.y, -3, 0); // 控制下降速度
                _rb.velocity = new Vector3(_rb.velocity.x, fallingSpeed, _rb.velocity.z);
                yield return null; // 等待下一帧
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private void StartThrust(InputAction.CallbackContext context)
        {
            if (_thrusterCoroutine != null && isThrusting) return;
            _thrusterCoroutine = StartCoroutine(ThrustCoroutine());
            isThrusting = true;
        }

        private void StopThrust(InputAction.CallbackContext context)
        {
            if (_thrusterCoroutine == null) return;
            StopCoroutine(_thrusterCoroutine);
            _thrusterCoroutine = null;
            isThrusting = false;
        }
    }
}
