using System;
using UnityEngine;
using Attribute = Utilities.Attribute;
using EventHandler = Utilities.EventHandler;

namespace Player
{
    public class AttributePanel : MonoBehaviour
    {
        public GameObject healthIconPrefab;
        public GameObject energyIconPrefab;
        public Transform healthContainer;
        public Transform energyContainer;

        private PlayerAttribute _playerAttribute;

        private void Awake()
        {
            _playerAttribute = GameObject.FindWithTag("Player").GetComponent<PlayerAttribute>();
        }

        private void OnEnable()
        {
            EventHandler.updateAttributePanel += UpdatePanel;
        }

        private void OnDisable()
        {
            EventHandler.updateAttributePanel -= UpdatePanel;
        }

        private void Start()
        {
            for (var i = 0; i < _playerAttribute.health; i++)
            {
                Instantiate(healthIconPrefab, healthContainer);
            }
            
            for (var i = 0; i < _playerAttribute.energy; i++)
            {
                Instantiate(energyIconPrefab, energyContainer);
            }
        }

        private void UpdatePanel(Attribute attribute, bool isAdding)
        {
            if (isAdding)
            {
                switch (attribute)
                {
                    case Attribute.Health:
                        Instantiate(healthIconPrefab, healthContainer);
                        break;
                    case Attribute.Energy:
                        Instantiate(energyIconPrefab, energyContainer);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(attribute), attribute, null);
                }
            }
            else
            {
                switch (attribute)
                {
                    case Attribute.Health:
                        if (_playerAttribute.health < 1) break; 
                        Destroy(healthContainer.GetChild(0).gameObject);
                        break;
                    case Attribute.Energy:
                        if (_playerAttribute.energy < 1) break;
                        Destroy(energyContainer.GetChild(0).gameObject);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(attribute), attribute, null);
                } 
            }
        }
    }
}
