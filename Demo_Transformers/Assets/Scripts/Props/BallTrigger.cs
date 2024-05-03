using System;
using UnityEngine;
using UnityEngine.Events;

namespace Props
{
    public class BallTrigger : MonoBehaviour
    {
        public UnityEvent onEnterEvent;
        public UnityEvent onExitEvent;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("RollingBall"))
            {
                onEnterEvent?.Invoke();
                other.GetComponent<RollingBall>().enabled = false;
                // Debug.Log("...");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("RollingBall"))
            {
                onExitEvent?.Invoke();
                other.GetComponent<RollingBall>().enabled = true;
            }
        }
    }
}
