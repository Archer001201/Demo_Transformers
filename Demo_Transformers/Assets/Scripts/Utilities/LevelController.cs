using DataSO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class LevelController : MonoBehaviour
    {
        public LevelDataSO levelData;
        
        public void LoadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void ClearData()
        {
            levelData.hasSavedData = false;
        }
    }
}
