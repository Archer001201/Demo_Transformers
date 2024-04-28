using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Props
{
    public class EnemyClearTrigger : MonoBehaviour
    {
        public UnityEvent onEnemyClearedEvent;
        public List<EnemyController> enemies;

        private void Update()
        {
            if (enemies.Count < 1) onEnemyClearedEvent?.Invoke();

            for (var i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null) enemies.RemoveAt(i);
            }
        }
    }
}
