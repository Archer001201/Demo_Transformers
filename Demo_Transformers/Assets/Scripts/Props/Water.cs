using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Props
{
    public class Water : MonoBehaviour
    {
        private Coroutine _attackCoroutine;
        private PlayerAttribute _playerAttribute;
        
        private void Start()
        {
            var player = GameObject.FindWithTag("Player");
            _playerAttribute = player.GetComponent<PlayerAttribute>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (_attackCoroutine != null) return;
                _attackCoroutine = StartCoroutine(AttackCoroutine());
            }
        }

        private void OnTriggerExit(Collider other)
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
