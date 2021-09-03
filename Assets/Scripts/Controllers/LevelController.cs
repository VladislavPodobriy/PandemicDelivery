using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LevelController : Singleton<LevelController>
    {
        [SerializeField] private ProgressBar _progressBar;
        [SerializeField] private Button _exitBtn;
        [SerializeField] private List<Sprite> _backgroundSprites;
        [SerializeField] private Transform _root;
        [SerializeField] private Transform _scoreTransform;
        [SerializeField] private CharacterController _character;

        private readonly List<Transform> _backgrounds = new List<Transform>();
        private TweenBase _speedTween;
        private TweenBase _timeTween;

        private float _speed;
        private bool _timeout;
        private static int _score;
        private LevelModel _level;

        public void Start()
        {
            _level = LevelDataManager.GetCurrentLevel();
            if (_level == null)
                return;
        
            _timeout = false;

            _scoreTransform.gameObject.SetActive(LevelDataManager.IsEndless());
            _progressBar.gameObject.SetActive(!LevelDataManager.IsEndless());
        
            _exitBtn.onClick.AddListener(ExitGame);
            _character.OnDeath.AddListener(CharacterDeath);
        
            SpawnBackground(0);
            SpawnBackground(26.6f);
        
            _speed = _level.StartSpeed;
            _speedTween = Tween.Value(_level.StartSpeed, _level.EndSpeed, val => _speed = val, _level.Duration, 0);
        
            if (!LevelDataManager.IsEndless())
            {
                _timeTween = Tween.Value(0, _level.Duration, 
                    value => _progressBar.UpdateView((float) value / _level.Duration), 
                    _level.Duration, 0, completeCallback: Timeout);
            }

            StartCoroutine(UpdateBackground());
            StartCoroutine(SpawnLevelObjects());
            StartCoroutine(SpawnLutObjects());
        }

        private IEnumerator UpdateBackground()
        {
            while (true)
            {
                _root.position = new Vector3(_root.position.x - _speed / 10, _root.position.y, 0);

                if (_backgrounds[1].position.x <= -26.6f)
                {
                    Destroy(_backgrounds[1].gameObject);
                    SpawnBackground(_backgrounds[0].position.x + 26.6f);
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator SpawnLevelObjects()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(1f, 3f) / _speed);
                var instance = Instantiate(GetRandomLevelObject(), _root);
                instance.transform.position = new Vector3(20, 0, 0);
            }
        }

        private IEnumerator SpawnLutObjects()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 1f) / _speed);
                var lut = GetRandomLutObject();
                if (lut != null)
                {
                    var instance = Instantiate(lut, _root);
                    instance.transform.position = new Vector3(30, Random.Range(0f, 2f), 0);
                }
            }
        }

        private void SpawnBackground(float pos)
        {
            var backgroundSprite = NoraCollections.GetRandomFromList(_backgroundSprites);

            var background = new GameObject("Background", typeof(SpriteRenderer)).transform;
            background.position = new Vector2(pos, 4f);
            background.parent = _root;
            background.GetComponent<SpriteRenderer>().sprite = backgroundSprite;

            _backgrounds.Insert(0, background);
        }

        private void Timeout()
        {
            if (_timeout)
                return;
        
            _timeout = true;
                    
            StopGame();
        
            if (LevelDataManager.GetCurrentLevelIndex() == PlayerDataManager.PlayerData.Progress)
            {
                PlayerDataManager.PlayerData.Progress++;
                PlayerDataManager.SaveProgress();
            }
        
            GameController.Win();
        }
    
        private async void CharacterDeath()
        {
            if (LevelDataManager.IsEndless())
            {
                if (PlayerDataManager.EndlessScore < _score)
                    PlayerDataManager.EndlessScore = _score;
            }
            else
            {
                PlayerDataManager.PlayerData.Hp--;
            }
            PlayerDataManager.SaveProgress();
            
            await Task.Delay(800);
            
            StopGame();
            
            await Task.Delay(2000);

            GameController.Lose();
        }
    
        private void StopGame()
        {
            Instance.StopAllCoroutines();

            Instance._speedTween?.Stop();
            Instance._timeTween?.Stop();
        }

        private void ExitGame()
        {
            if (LevelDataManager.IsEndless())
            {
                if (_score > PlayerDataManager.EndlessScore)
                {
                    PlayerDataManager.EndlessScore = _score;
                    PlayerDataManager.SaveProgress();
                }
            }
            else
            {
                PlayerDataManager.PlayerData.Hp--;
            
                if (PlayerDataManager.PlayerData.Hp <= 0)
                    PlayerDataManager.ResetProgress();
                else
                    PlayerDataManager.SaveProgress();
            }
        
            GameController.ToMainMenu();
        }
    
        private Transform GetRandomLutObject()
        {
            var chancesSum = _level.LutObjects.Select(x => x.Chance).Sum();
            float randomChance = Random.Range(0, chancesSum);

            float totalChance = 0;
            foreach (var lutObject in _level.LutObjects)
            {
                totalChance += lutObject.Chance;
                if (totalChance > randomChance)
                    return LevelDataManager.LutObjects.FirstOrDefault(x => x.Name == lutObject.Name)?.Transform;
            }

            return null;
        }
    
        private Transform GetRandomLevelObject()
        {
            List<LevelObjectModel> availableObjects;
            if (LevelDataManager.IsEndless())
                availableObjects = _level.LevelObjects.Where(x => x.MinimalSpeed < _speed).ToList();
            else
                availableObjects = _level.LevelObjects;
        
            var chancesSum = availableObjects.Select(x => x.Chance).Sum();
            float randomChance = Random.Range(0, chancesSum);

            float totalChance = 0;
            foreach (var levelObject in availableObjects)
            {
                totalChance += levelObject.Chance;
                if (totalChance > randomChance)
                {
                    return LevelDataManager.LevelObjects.FirstOrDefault(x => x.Name == levelObject.Name)?.Transform;
                }
            }

            return null;
        }

        public static void AddScore(int value)
        {
            _score += value;
        }
    
        public static int GetScore()
        {
            return _score;
        }
    }
}
