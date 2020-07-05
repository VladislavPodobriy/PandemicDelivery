using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nora;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class LevelController : Singleton<LevelController>
{
    [SerializeField] private Button _exitBtn;
    [SerializeField] private List<Sprite> _backgroundSprites;
    [SerializeField] private Transform _root;
    [SerializeField] private Transform hero;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private List<Transform> _backgrounds;
    [SerializeField] private Transform scoreTransform;

    [SerializeField] private float _speed;

    private TweenBase _speedTween;
    private TweenBase _timeTween;

    public static int Score;

    public bool end;

    public void Start()
    {
        end = false;

        scoreTransform.gameObject.SetActive(LevelManager.IsEndless);

        _exitBtn.onClick.AddListener(()=>
        {
            if (!LevelManager.IsEndless)
                GameManager.Exit();
            else
                GameManager.OnEndlessEnd();
        });

        _backgrounds.Add(SpawnBackground(26.6f));
        _backgrounds.Add(SpawnBackground(0));

        if (!LevelManager.IsEndless)
        {
            _speed = LevelManager.Level.Speed.x;

            _speedTween = Tween.Value(LevelManager.Level.Speed.x, LevelManager.Level.Speed.y, val => _speed = val,
                LevelManager.Level.Duration, 0);
            _timeTween = Tween.Value(0, LevelManager.Level.Duration,
                value => _progressBar.UpdateProgress((float)value / LevelManager.Level.Duration),
                LevelManager.Level.Duration, 0, completeCallback: () =>
                {
                    if (end)
                        return;

                    StopGame();
                    GameManager.OnWin();

                    end = true;
                });
        }
        else
        {
            _speed = 1;
            StartCoroutine(UpdateSpeed());
            _progressBar.gameObject.SetActive(false);
        }

        StartCoroutine(UpdateLevel());
        StartCoroutine(Spawn());
        StartCoroutine(SpawnLut());
    }

    public static void StopGame()
    {
        Instance.StopAllCoroutines();

        Instance._speedTween?.Stop();
        Instance._timeTween?.Stop();
    }

    private IEnumerator UpdateSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            _speed += 0.01f;
            LutController.AddPaper();
        }
    }

    private IEnumerator UpdateLevel()
    {
        while (true)
        {
            _root.position = new Vector3(_root.position.x - _speed / 10, _root.position.y, 0);

            if (_backgrounds[1].position.x <= -26.6f)
            {
                Destroy(_backgrounds[1].gameObject);

                var background = SpawnBackground(_backgrounds[0].position.x + 26.6f);

                _backgrounds.Insert(0, background);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f) / _speed);

            Transform prefab;
            if (!LevelManager.IsEndless)
                prefab = Collections.GetRandomWithChance(LevelManager.Level.Characters);
            else
            {
                var characters = LevelManager.Endless.Characters.Where(ch => _speed >= ch.Value[1])
                    .Select(ch => new {ch.Key, ch.Value}).ToDictionary(a => a.Key, a => (int)a.Value[0]);

                prefab = Collections.GetRandomWithChance(characters);
            }

            var instance = Instantiate(prefab, _root);
            instance.transform.position = new Vector3(20, 0, 0);
        }
    }

    private IEnumerator SpawnLut()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 1f) / _speed);
            Transform prefab;
            if (!LevelManager.IsEndless)
                prefab = Collections.GetRandomWithChance(LevelManager.Level.Lut);
            else
                prefab = Collections.GetRandomWithChance(LevelManager.Endless.Lut);

            if (prefab == null)
                continue;

            var instance = Instantiate(prefab, _root);
            instance.transform.position = new Vector3(30, Random.Range(0f, 2f), 0);
        }
    }

    private Transform SpawnBackground(float pos)
    {
        var backgroundSprite = Collections.GetRandomFromList(_backgroundSprites);

        var background = new GameObject("Background", typeof(SpriteRenderer)).transform;
        background.position = new Vector2(pos, 4f);
        background.parent = _root;
        background.GetComponent<SpriteRenderer>().sprite = backgroundSprite;

        return background;
    }
}
