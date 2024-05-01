using System;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Props
{
    public class MovableItem : MonoBehaviour
    {
        public UnityEvent onArrivedDestinationEvent;
        public Vector3 destination;
        public float speed;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            MoveToDestination();
        }

        public void MoveToDestination()
        {
            if (_rb == null) return;  // 确保Rigidbody组件存在

            // 计算每帧移动的向量
            var moveAmount = speed * Time.fixedDeltaTime;
            // 计算新位置，向目标位置移动
            var newPosition = Vector3.MoveTowards(transform.position, destination, moveAmount);
        
            _rb.MovePosition(newPosition);

            // 检查是否到达目标位置
            if (Vector3.Distance(transform.position, destination) < 0.001f)
            {
                // 如果到达目标位置，停止移动
                _rb.velocity = Vector3.zero;
                onArrivedDestinationEvent?.Invoke();
            }
        }

        public void SetDestination(Transform newDestination)
        {
            destination = newDestination.position;
        }
    }
}
