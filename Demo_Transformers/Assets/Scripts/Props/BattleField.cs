using System;
using System.Collections.Generic;
using UnityEngine;

namespace Props
{
    public class BattleField : MonoBehaviour
    {
        public List<EnemyController> enemies;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                foreach (var e in enemies)
                {
                    e.isChasing = true;
                }

                this.enabled = false;
            }
        }
    }
}
