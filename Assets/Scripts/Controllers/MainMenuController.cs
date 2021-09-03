using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MainMenuController : MonoBehaviour
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
            PlayerDataManager.InitGame();

            for (int i = 0; i < _buttons.Count; i++)
            {
                var index = i;
                _buttons[i].onClick.AddListener(() => LoadLevel(index));
            }

            _newGameBtn.onClick.AddListener(NewGame);
            _endlessBtn.onClick.AddListener(LoadEndless);
        }

        private void Start()
        {
            UpdateView();
        }

        private void NewGame()
        {
            PlayerDataManager.ResetProgress();
            UpdateView();
        }

        private void UpdateView()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                if (i > PlayerDataManager.PlayerData.Progress)
                {
                    _buttons[i].image.sprite = _closedSprite;
                    _buttons[i].interactable = false;
                }
                else
                {
                    var orderIndex = PlayerDataManager.PlayerData.OrdersList[i];
                    _buttons[i].image.sprite = _icons[orderIndex];
                    _buttons[i].interactable = true;
                }
            }

            _scoreText.SetText(PlayerDataManager.EndlessScore.ToString());

            for (int i = 0; i < 3; i++)
            {
                _hearts[i].SetActive(i < PlayerDataManager.PlayerData.Hp);
            }
        }

        private void LoadLevel(int index)
        {
            LevelDataManager.SetCurrentLevel(index);
            SceneManager.LoadScene(1);
        }

        private void LoadEndless()
        {
            LevelDataManager.SetEndless();
            SceneManager.LoadScene(1);
        }
    }
}
