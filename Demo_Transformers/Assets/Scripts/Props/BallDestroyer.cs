using System;
using UnityEngine;

namespace Props
{
    public class BallDestroyer : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("RollingBall")) Destroy(other.gameObject);
        }
    }
}
