using System;
using UnityEngine;
using UnityEngine.Events;

namespace Props
{
    public class CollisionTrigger : MonoBehaviour
    {
        public UnityEvent onCollisionEnterEvent;
        public UnityEvent onCollisionExitEvent;
        public GameObject target;
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject == target)
            {
                onCollisionEnterEvent?.Invoke();
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject == target)
            {
                onCollisionExitEvent?.Invoke();
            }
        }
    }
}
