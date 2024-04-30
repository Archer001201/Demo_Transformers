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

        private void Awake()
        {
            var player = GameObject.FindWithTag("Player");
            _playerAttribute = player.GetComponent<PlayerAttribute>();
            _playerController = player.GetComponent<PlayerController>();
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
            for (var i = 0; i < healthDelta; i++)
            {
                EventHandler.updateAttributePanel(Attribute.Health, true);
            }
            for (var i = 0; i < energyDelta; i++)
            {
                EventHandler.updateAttributePanel(Attribute.Energy, true);
            }
            _playerController.ChangeCurrentModule();
            levelData.hasSavedData = true;
            onAfterSavingEvent?.Invoke();
        }
    }
}
