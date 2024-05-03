using System;
using System.Collections.Generic;
using DataSO;
using UnityEngine;
using Utilities;
using Attribute = Utilities.Attribute;
using EventHandler = Utilities.EventHandler;

namespace Player
{
    public class PlayerAttribute : MonoBehaviour
    {
        public int health = 3;
        public int energy = 3;
        public Module currentModule;
        public Module clipboard;
        public List<Module> history;
        public LevelDataSO levelData;
        // public int historyAmount = 1;

        private void Start()
        {
            levelData.LoadLevelData();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("EnergySupply")) return;
            Destroy(other.gameObject);
            energy++;
            EventHandler.OnUpdateAttributePanel(Attribute.Energy, true);
        }

        public void TakeDamage()
        {
            health--;
            EventHandler.OnUpdateAttributePanel(Attribute.Health, false);
        }
    }
}
