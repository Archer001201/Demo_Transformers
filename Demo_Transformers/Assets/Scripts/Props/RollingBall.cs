using System;
using UnityEngine;

namespace Props
{
    public class RollingBall : MonoBehaviour
    {
        public float duration;

        private void FixedUpdate()
        {
            if (duration < 0) Destroy(gameObject);
            duration -= Time.fixedDeltaTime;
        }
    }
}
