using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Sprite _closedSprite;
    [SerializeField] private List<Sprite> _icons;
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private List<GameObject> _hearts;
    [SerializeField] private Button _newGameBtn;
    [SerializeField] private Button _endlessBtn;

    public void Awake()
    {
        DataManager.InitGame();

        for (int i = 0; i < _buttons.Count; i++)
        {
            var index = i;
            _buttons[i].onClick.AddListener(() => LoadLevel(index));
        }

        _newGameBtn.onClick.AddListener(NewGame);
        _endlessBtn.onClick.AddListener(()=>LoadEndless());
    }

    void Start()
    {
        UpdateView();
    }

    public void NewGame()
    {
        DataManager.ResetProgress();
        UpdateView();
    }

    public void UpdateView()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            if (i > DataManager.Data.Progress)
            {
                _buttons[i].image.sprite = _closedSprite;
                _buttons[i].interactable = false;
            }
            else
            {
                var orderIndex = DataManager.Data.OrdersList[i];
                _buttons[i].image.sprite = _icons[orderIndex];
                _buttons[i].interactable = true;
            }
        }

        _scoreText.SetText(DataManager.Score.ToString());

        for (int i = 0; i < 3; i++)
        {
            _hearts[i].SetActive(i < DataManager.Data.Hp);
        }
    }

    public void LoadLevel(int index)
    {
        LevelManager.IsEndless = false;
        LevelManager.PrepareLevel(index);
        SceneManager.LoadScene(1);
    }

    public void LoadEndless()
    {
        LevelManager.IsEndless = true;
        SceneManager.LoadScene(1);
    }
}
