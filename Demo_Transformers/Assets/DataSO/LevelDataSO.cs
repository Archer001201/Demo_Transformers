using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace DataSO
{
    [CreateAssetMenu(fileName = "NewLevelData", menuName = "Level Data")]
    public class LevelDataSO : ScriptableObject
    {
        [Header("Player Data")]
        public Vector3 playerPosition;
        public int maxHealth;
        public int maxEnergy;
        public int historyRecords;
        // public int health;
        // public int energy;
        // public Module currentModule;
        // public Module clipboard;
        // public List<Module> history;

        // [Header("Level Data")] public List<ObjectData> gameItems;
    }
}
