using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Props
{
    public class EnemyController : MonoBehaviour
    {
        public bool isChasing;
        private NavMeshAgent _agent;
        private Transform _playerTrans;
        private PlayerAttribute _playerAttribute;
        private Coroutine _attackCoroutine;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            var player = GameObject.FindWithTag("Player");
            _playerTrans = player.transform;
            _playerAttribute = player.GetComponent<PlayerAttribute>();
        }

        private void Update()
        {
            if (isChasing) _agent.SetDestination(_playerTrans.position);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (_attackCoroutine != null) return;
                _attackCoroutine = StartCoroutine(AttackCoroutine());
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (_attackCoroutine == null) return;
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
        }

        private IEnumerator AttackCoroutine()
        {
            while (true)
            {
                _playerAttribute.TakeDamage();
                yield return new WaitForSeconds(1f);
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
