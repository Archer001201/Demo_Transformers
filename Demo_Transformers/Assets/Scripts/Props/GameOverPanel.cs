using System;
using UnityEngine;
using EventHandler = Utilities.EventHandler;

namespace Props
{
    public class GameOverPanel : MonoBehaviour
    {
        public GameObject gameOverPanel;

        private void Awake()
        {
            gameOverPanel.SetActive(false);
        }

        private void OnEnable()
        {
            EventHandler.gameOver += HandleGameOverPanel;
        }

        private void OnDisable()
        {
            EventHandler.gameOver -= HandleGameOverPanel;
        }

        public void HandleGameOverPanel()
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
