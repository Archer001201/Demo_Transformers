using System;
using UnityEngine;
using UnityEngine.Events;

namespace Props
{
    public class PlayerEnterTrigger : MonoBehaviour
    {
        public UnityEvent onPlayerEnterEvent;

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            onPlayerEnterEvent?.Invoke();
        }
    }
}
