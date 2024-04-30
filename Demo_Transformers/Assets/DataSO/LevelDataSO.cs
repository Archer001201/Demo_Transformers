using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Utilities;

namespace DataSO
{
    [CreateAssetMenu(fileName = "NewLevelData", menuName = "Level Data")]
    public class LevelDataSO : ScriptableObject
    {
        [Header("Player Data")]
        public Vector3 playerPosition;
        // public int maxHealth;
        // public int maxEnergy;
        public int historyRecords;
        public bool hasSavedData;
        // public int health;
        // public int energy;
        // public Module currentModule;
        // public Module clipboard;
        // public List<Module> history;

        // [Header("Level Data")] public List<ObjectData> gameItems;

        private PlayerAttribute _playerAttribute;

        // private void OnEnable()
        // {
        //     // _playerAttribute.health = health;
        //     // _playerAttribute.energy = energy;
        //     _playerAttribute = GameObject.FindWithTag("Player").GetComponent<PlayerAttribute>();
        // }

        public void LoadLevelData()
        {
            _playerAttribute = GameObject.FindWithTag("Player").GetComponent<PlayerAttribute>();
            if (hasSavedData)
            {
                _playerAttribute.transform.position = playerPosition;
            }
            else
            {
                historyRecords = 1;
            }
        }
    }
}
