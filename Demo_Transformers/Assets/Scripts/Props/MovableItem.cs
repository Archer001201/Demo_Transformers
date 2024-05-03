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
        public float moveSpeed;
        private Rigidbody _rb;
        public float deceleration = 1f;  // 减速度
        public float minSpeed = 0.01f;

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
            // if (_rb == null) return;  // 确保Rigidbody组件存在
            //
            // // 计算每帧移动的向量
            // var moveAmount = speed * Time.fixedDeltaTime;
            // // 计算新位置，向目标位置移动
            // var newPosition = Vector3.MoveTowards(transform.position, destination, moveAmount);
            //
            // _rb.MovePosition(newPosition);
            //
            // // 检查是否到达目标位置
            // if (Vector3.Distance(transform.position, destination) < 0.001f)
            // {
            //     // 如果到达目标位置，停止移动
            //     _rb.velocity = Vector3.zero;
            //     onArrivedDestinationEvent?.Invoke();
            // }
            
            if (_rb == null) return;  // 确保Rigidbody组件存在

            // 计算当前位置到目标位置的距离
            var distanceToDestination = Vector3.Distance(transform.position, destination);
            // 当接近目标点时开始减速
            if (distanceToDestination < 4f) {
                // 逐渐减速，直到达到最小速度
                moveSpeed = Mathf.Max(moveSpeed - deceleration * Time.fixedDeltaTime, minSpeed);
            }

            // 计算每帧移动的向量
            var moveAmount = moveSpeed * Time.fixedDeltaTime;
            // 计算新位置，向目标位置移动
            var newPosition = Vector3.MoveTowards(transform.position, destination, moveAmount);

            _rb.MovePosition(newPosition);

            // 检查是否到达目标位置
            if (distanceToDestination < 0.001f)
            {
                // 如果到达目标位置，停止移动并锁定速度为0
                _rb.velocity = Vector3.zero;
                moveSpeed = 0;  // 重置速度为0，以便下次使用时重新设置
                onArrivedDestinationEvent?.Invoke();
            }
        }

        public void SetDestination(Transform newDestination)
        {
            destination = newDestination.position;
            moveSpeed = speed;
        }
    }
}
