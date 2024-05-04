using System;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Props
{
    public class MagneticTrigger : MonoBehaviour
    {
        public UnityEvent onTriggerEnterEvent;
        public UnityEvent onTriggerExitEvent;
        public MagneticType type;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(type.ToString()))
            {
                onTriggerEnterEvent?.Invoke();
                // Debug.Log("Enter");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(type.ToString()))
            {
                onTriggerExitEvent?.Invoke();
                // Debug.Log("Exit");
            }
        }
    }
}
