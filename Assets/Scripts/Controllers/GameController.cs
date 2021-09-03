using System.Threading.Tasks;
using Pixelplacement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameController : Singleton<GameController>
    {
        [SerializeField] private Transform _winScreen;
        [SerializeField] private Transform _endWinScreen;
        [SerializeField] private Transform _endLoseScreen;
        [SerializeField] private Transform _loseScreen;

        public void Awake()
        {
            AdsController.OnVideoEnd(ShowLoseScreen);
        }

        public static void ToMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public static async void Win()
        {
            if (LevelDataManager.GetCurrentLevelIndex() == 10)
                ShowEndGameScreen();
            else
                ShowWinScreen();
        
            await Task.Delay(3000);
            ToMainMenu();
        }

        public static async void Lose()
        {
            AdsController.N++;
            if (AdsController.N == 3)
            {
                AdsController.N = 0;
                AdsController.ShowVideo();
            }
            else
            {
                ShowLoseScreen();
                await Task.Delay(3000);
                ToMainMenu();
            }
        }

        private static void ShowEndGameScreen()
        {
            Instance._endWinScreen.gameObject.SetActive(true);
        }
    
        private static void ShowWinScreen()
        {
            Instance._winScreen.gameObject.SetActive(true);
        }
    
        private static void ShowLoseScreen()
        {
            if (PlayerDataManager.PlayerData.Hp > 0)
                Instance._loseScreen.gameObject.SetActive(true);
            else
            {
                Instance._endLoseScreen.gameObject.SetActive(true);
                PlayerDataManager.ResetProgress();
            }
        }
    }
}
