using System;
using UnityEngine;

namespace Props
{
    public class BallGenerator : MonoBehaviour
    {
        public GameObject ballPrefab;
        public GameObject ballInstance;
        public Transform startPosition;
        public GameObject block;

        private void Update()
        {
            if (ballInstance == null && block == null)
            {
                ballInstance = Instantiate(ballPrefab, startPosition.position, Quaternion.identity);
            }
        }
    }
}
