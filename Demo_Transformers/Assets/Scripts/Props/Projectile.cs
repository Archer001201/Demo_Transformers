using System;
using UnityEngine;

namespace Props
{
    public class Projectile : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Destroyable") || 
                other.gameObject.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
