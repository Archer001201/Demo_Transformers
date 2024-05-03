using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Props
{
    public class CollisionTrigger : MonoBehaviour
    {
        public UnityEvent onCollisionEnterEvent;
        public UnityEvent onCollisionExitEvent;
        public string targetTag;
        public GameObject target;
        public List<Gun> guns;
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject == target || other.gameObject.CompareTag(targetTag))
            {
                onCollisionEnterEvent?.Invoke();
                // Debug.Log("collide");
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject == target || other.gameObject.CompareTag(targetTag))
            {
                onCollisionExitEvent?.Invoke();
            }
        }

        public void StartFire(bool result)
        {
            foreach (var gun in guns)
            {
                if (result) gun.StartFire();
                else gun.StopFire();
            }
        }
    }
}
