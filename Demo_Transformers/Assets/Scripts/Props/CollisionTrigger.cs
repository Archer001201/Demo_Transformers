using System;
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
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject == target || other.gameObject.CompareTag(targetTag))
            {
                onCollisionEnterEvent?.Invoke();
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject == target || other.gameObject.CompareTag(targetTag))
            {
                onCollisionExitEvent?.Invoke();
            }
        }
    }
}
