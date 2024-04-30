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
            var healthDelta = levelData.maxHealth - _playerAttribute.health;
            var energyDelta = levelData.maxEnergy - _playerAttribute.energy;
            _playerAttribute.health = levelData.maxHealth;
            _playerAttribute.energy = levelData.maxEnergy;
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
            onAfterSavingEvent?.Invoke();
        }
    }
}
