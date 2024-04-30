using System;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Props
{
    public class MagneticTrigger : MonoBehaviour
    {
        public UnityEvent onTriggeredEvent;
        public MagneticType type;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(type.ToString()))
            {
                onTriggeredEvent?.Invoke();
            }
        }
    }
}
