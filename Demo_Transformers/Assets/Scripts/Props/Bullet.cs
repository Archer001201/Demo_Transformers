using System;
using Player;
using UnityEngine;

namespace Props
{
    public class Bullet : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerAttribute>().TakeDamage();
            }
            Destroy(gameObject);
        }
    }
}
