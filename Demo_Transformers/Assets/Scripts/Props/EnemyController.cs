using System;
using UnityEngine;
using UnityEngine.AI;

namespace Props
{
    public class EnemyController : MonoBehaviour
    {
        public bool isChasing;
        
        private NavMeshAgent _agent;
        private Transform _playerTrans;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _playerTrans = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            if (isChasing) _agent.SetDestination(_playerTrans.position);
        }
    }
}
