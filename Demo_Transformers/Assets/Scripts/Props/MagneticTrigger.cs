using System;
using UnityEngine;
using UnityEngine.Events;

namespace Props
{
    public class MagneticTrigger : MonoBehaviour
    {
        public UnityEvent onTriggeredEvent;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Magnetic"))
            {
                onTriggeredEvent?.Invoke();
            }
        }
    }
}
