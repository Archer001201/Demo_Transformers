using System;
using UnityEngine;

namespace Props
{
    public class Projectile : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
        }
    }
}
