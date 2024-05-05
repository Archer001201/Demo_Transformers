using System;
using DataSO;
using Player;
using UnityEngine;
using UnityEngine.Events;
using Utilities;
using Attribute = Utilities.Attribute;
using EventHandler = Utilities.EventHandler;

namespace Props
{
    public class SavingPoint : MonoBehaviour
    {
        public int maxHealth = 3;
        public int maxEnergy = 1;
        public UnityEvent onAfterSavingEvent;
        public LevelDataSO levelData;
        private PlayerAttribute _playerAttribute;
        private PlayerController _playerController;
        public GameObject hud;
        private Transform _cameraTrans;

        private void Awake()
        {
            var player = GameObject.FindWithTag("Player");
            _playerAttribute = player.GetComponent<PlayerAttribute>();
            _playerController = player.GetComponent<PlayerController>();
            hud.SetActive(false);
            _cameraTrans = GameObject.FindWithTag("MainCamera").transform;
        }

        private void Update()
        {
            if (hud.activeSelf)
            {
                hud.transform.LookAt(_cameraTrans);
                hud.transform.Rotate(0,180,0);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            hud.SetActive(true);
            SaveAndRecover();
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            hud.SetActive(false);
        }

        public void SaveAndRecover()
        {
            levelData.playerPosition = _playerAttribute.transform.position;
            var healthDelta = maxHealth - _playerAttribute.health;
            var energyDelta = maxEnergy - _playerAttribute.energy;
            // _playerAttribute.health = levelData.health;
            // _playerAttribute.energy = levelData.energy;
            _playerAttribute.health = maxHealth;
            _playerAttribute.energy = maxEnergy;
            _playerAttribute.currentModule = Module.Empty;
            EventHandler.updateCurrentText(_playerAttribute.currentModule);
            _playerAttribute.clipboard = Module.Empty;
            EventHandler.updateClipboardText(_playerAttribute.clipboard);
            for (var i = _playerAttribute.history.Count-1; i >= 0; i--)
            {
                _playerAttribute.history.RemoveAt(i);
                EventHandler.removeModuleFromHistory(i);
            }

            if (healthDelta > 0)
            {
                for (var i = 0; i < healthDelta; i++)
                {
                    EventHandler.updateAttributePanel(Attribute.Health, true);
                } 
            }
            else
            {
                for (var i = 0; i < Mathf.Abs(healthDelta); i++)
                {
                    EventHandler.updateAttributePanel(Attribute.Health, false);
                } 
            }

            if (energyDelta > 0)
            {
                for (var i = 0; i < energyDelta; i++)
                {
                    EventHandler.updateAttributePanel(Attribute.Energy, true);
                } 
            }
            else
            {
                for (var i = 0; i < Mathf.Abs(energyDelta); i++)
                {
                    EventHandler.updateAttributePanel(Attribute.Energy, false);
                } 
            }
           
            _playerController.ChangeCurrentModule();
            levelData.hasSavedData = true;
            onAfterSavingEvent?.Invoke();
        }
    }
}
