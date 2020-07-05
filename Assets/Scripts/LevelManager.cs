using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pixelplacement;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] public List<Transform> Characters;
    [SerializeField] public List<Transform> Lut;

    private static List<LevelSettings> _levels;
    private static EndlessSettings _endless;

    public static class Level
    {
        public static int Index;
        public static int Duration;
        public static Vector2 Speed;
        public static Dictionary<Transform, int> Characters;
        public static Dictionary<Transform, int> Lut;
    }

    public static class Endless
    {
        public static Dictionary<Transform, float[]> Characters;
        public static Dictionary<Transform, int> Lut;
    }

    public static bool IsEndless = false;

    private string _path;
    
    public void Awake()
    {
        try
        {
            var file = Resources.Load<TextAsset>("levels");
            var json = file.ToString();

            _levels = JsonConvert.DeserializeObject<List<LevelSettings>>(json);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

        try
        {
            var file = Resources.Load<TextAsset>("endless");
            var json = file.ToString();

            _endless = JsonConvert.DeserializeObject<EndlessSettings>(json);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        PrepareEndless();
    }

    public static void PrepareLevel(int index)
    {
        Level.Index = index;
        Level.Duration = _levels[index].Duration;
        Level.Speed = new Vector2(_levels[index].StartSpeed, _levels[index].EndSpeed);
        Level.Characters = new Dictionary<Transform, int>();
        Level.Lut = new Dictionary<Transform, int>();

        foreach (var pair in _levels[index].Characters)
        {
             var obj = Instance.Characters.FirstOrDefault(character => character.name == pair.Key);

             if (obj != null)
                 Level.Characters.Add(obj, pair.Value);
        }

        foreach (var pair in _levels[index].Lut)
        {
            var obj = Instance.Lut.FirstOrDefault(lut => lut.name == pair.Key);

            if (obj != null)
                Level.Lut.Add(obj, pair.Value);
        }
    }

    public static void PrepareEndless()
    {
        Endless.Characters = new Dictionary<Transform, float[]>();
        Endless.Lut = new Dictionary<Transform, int>();

        foreach (var pair in _endless.Characters)
        {
            var obj = Instance.Characters.FirstOrDefault(character => character.name == pair.Key);

            if (obj != null)
                Endless.Characters.Add(obj, pair.Value);
        }

        foreach (var pair in _endless.Lut)
        {
            var obj = Instance.Lut.FirstOrDefault(lut => lut.name == pair.Key);

            if (obj != null)
                Endless.Lut.Add(obj, pair.Value);
        }
    }
}
