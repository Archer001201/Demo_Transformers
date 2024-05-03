using System;
using System.Collections;
using UnityEngine;
// using Vector3 = System.Numerics.Vector3;

namespace Props
{
    public class Gun : MonoBehaviour
    {
        public GameObject bullet;
        public float fireSpeed;
        public float fireForce;
        public Transform firePosition;
        private Coroutine _fireCoroutine;
        public bool canMove;
        private Rigidbody _rb;
        public Vector3 position1;
        public Vector3 position2;
        public bool isPosition1;
        public float speed;
        public Vector3 destination;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!canMove || _rb == null)
            {
                return;
            }
            destination = isPosition1 ? position1 : position2;
            // 计算每帧移动的向量
            var moveAmount = speed * Time.fixedDeltaTime;
            // 计算新位置，向目标位置移动
            // var newPosition = Vector3.(transform.position, destination, moveAmount);
            var newPosition = Vector3.MoveTowards(transform.position, destination, moveAmount);
            _rb.MovePosition(newPosition);
            // Debug.Log("move");
            
            if (Vector3.Distance(transform.position, destination) < 0.001f)
            {
                isPosition1 = !isPosition1;
            }
        }

        private IEnumerator FireCoroutine()
        {
            while (true)
            {
                var go = Instantiate(bullet, firePosition.position, Quaternion.identity);
                go.GetComponent<Rigidbody>().AddForce(transform.forward*fireForce, ForceMode.Impulse);
                yield return new WaitForSeconds(fireSpeed);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        public void StartFire()
        {
            if (_fireCoroutine != null) return;
            _fireCoroutine = StartCoroutine(FireCoroutine());
        }

        public void StopFire()
        {
            if (_fireCoroutine == null) return;
            StopCoroutine(_fireCoroutine);
            _fireCoroutine = null;
        }
    }
}
