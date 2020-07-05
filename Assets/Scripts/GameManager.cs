using System.Collections;
using Pixelplacement;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Transform _winScreen;
    [SerializeField] private Transform _endWinScreen;
    [SerializeField] private Transform _endLoseScreen;
    [SerializeField] private Transform _loseScreen;
    [SerializeField] private TextMeshProUGUI _loseScore;

    public void Awake()
    {
        AdsController.Instance.Video.onFinish.AddListener(ShowLoseScreen);
    }

    public static void OnEndlessEnd()
    {
        if (LevelController.Score > DataManager.Score)
        {
            DataManager.Score = LevelController.Score;
            DataManager.SaveProgress();
        }

        SceneManager.LoadScene(0);
    }

    public static void OnWin()
    {
        if (LevelManager.Level.Index == DataManager.Data.Progress)
        {
            DataManager.Data.Progress++;
            DataManager.SaveProgress();
        }

        if (LevelManager.Level.Index == 10)
            Instance._endWinScreen.gameObject.SetActive(true);
        else
            Instance._winScreen.gameObject.SetActive(true);

        Instance.StartCoroutine(Instance.GoToMenu());
    }

    public static void OnLose()
    {
        DataManager.Data.Hp--;
        DataManager.SaveProgress();

        AdsController.N++;
        if (AdsController.N == 3)
        {
            AdsController.N = 0;
            AdsController.ShowVideo();
        }
        else
        {
            ShowLoseScreen();
        }
    }

    public static void ShowLoseScreen()
    {
        if (DataManager.Data.Hp > 0)
            Instance._loseScreen.gameObject.SetActive(true);
        else
        {
            Instance._endLoseScreen.gameObject.SetActive(true);
            DataManager.ResetProgress();
        }

        Instance.StartCoroutine(Instance.GoToMenu());
    }

    public static void Exit()
    {
        DataManager.Data.Hp--;
        DataManager.SaveProgress();

        if (DataManager.Data.Hp <= 0)
            DataManager.ResetProgress();

        SceneManager.LoadScene(0);
    }

    private IEnumerator GoToMenu()
    {
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
