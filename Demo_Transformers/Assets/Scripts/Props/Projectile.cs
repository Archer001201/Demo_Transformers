using System;
using UnityEngine;

namespace Props
{
    public class Projectile : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Destroyable") || other.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
