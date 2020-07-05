using System.Collections;
using System.Collections.Generic;
using Nora;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;
using UnityEngine.UI;

public class EndlessMode : Singleton<EndlessMode>
{
    [SerializeField] private Button _exitBtn;
    [SerializeField] private List<Sprite> _backgroundSprites;
    [SerializeField] private Transform _root;
    [SerializeField] private Transform hero;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private List<Transform> _backgrounds;

    public bool Endless = false;

    private float _speed;

    private TweenBase _speedTween;
    private TweenBase _timeTween;

    public static int Paper;

    public bool end;

    public void Start()
    {
        end = false;

        _exitBtn.onClick.AddListener(GameManager.Exit);

        _backgrounds.Add(SpawnBackground(20));
        _backgrounds.Add(SpawnBackground(0));

        _speed = LevelManager.Level.Speed.x;

        if (!Endless)
        {
            _speedTween = Tween.Value(LevelManager.Level.Speed.x, LevelManager.Level.Speed.y, val => _speed = val,
                LevelManager.Level.Duration, 0);
            _timeTween = Tween.Value(0, LevelManager.Level.Duration,
                value => _progressBar.UpdateProgress((float) value / LevelManager.Level.Duration),
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
            StartCoroutine(UpdateSpeed());
        }

        StartCoroutine(UpdateLevel());
        StartCoroutine(Spawn());
        StartCoroutine(SpawnLut());

        Paper = 0;
    }

    public static void StopGame()
    {
        Instance.StopAllCoroutines();

        Instance._speedTween.Stop();
        Instance._timeTween.Stop();
    }

    private IEnumerator UpdateSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            _speed += 0.01f;
        }
    }

    private IEnumerator UpdateLevel()
    {
        while (true)
        {
            _root.position = new Vector3(_root.position.x - _speed / 10, _root.position.y, 0);

            if (_backgrounds[1].position.x <= -20)
            {
                Destroy(_backgrounds[1].gameObject);

                var background = SpawnBackground(_backgrounds[0].position.x + 20);

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

            var prefab = Collections.GetRandomWithChance(LevelManager.Level.Characters);

            var instance = Instantiate(prefab, _root);
            instance.transform.position = new Vector3(20, 0, 0);
        }
    }

    private IEnumerator SpawnLut()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 1f) / _speed);

            var prefab = Collections.GetRandomWithChance(LevelManager.Level.Lut);


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
        background.position = new Vector2(pos, 3f);
        background.parent = _root;
        background.GetComponent<SpriteRenderer>().sprite = backgroundSprite;

        return background;
    }
}
