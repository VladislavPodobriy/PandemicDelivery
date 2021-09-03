using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pixelplacement;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelDataManager : Singleton<LevelDataManager>
    {
        [SerializeField] private List<Transform> _lutObjectPrefabs;
        [SerializeField] private List<Transform> _levelObjectPrefabs;
        
        public static List<GameItem> LevelObjects;
        public static List<GameItem> LutObjects;
        
        private static DataModel _data;
        private static  LevelModel _currentLevel;
        private static int _currentLevelIndex;
        private static bool _isEndless;
        
        public void Awake()
        {
            LevelObjects = _levelObjectPrefabs
                .Select(x => new GameItem {Name = x.name, Transform = x})
                .ToList();

            LutObjects = _lutObjectPrefabs
                .Select(x => new GameItem {Name = x.name, Transform = x})
                .ToList();
            
            try
            {
                var file = Resources.Load<TextAsset>("levels");
                var json = file.ToString();
                _data = JsonConvert.DeserializeObject<DataModel>(json);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public static int GetCurrentLevelIndex()
        {
            return _currentLevelIndex;
        }
    
        public static void SetCurrentLevel(int index)
        {
            _currentLevelIndex = index;
            index = index >= 0 && index < _data.Levels.Count ? index : 0;
            _currentLevel = _data.Levels[index];
            _isEndless = false;
        }

        public static void SetEndless()
        {
            _currentLevel = _data.Endless;
            _isEndless = true;
        }

        public static LevelModel GetCurrentLevel()
        {
            return _currentLevel;
        }

        public static bool IsEndless()
        {
            return _isEndless;
        }
    }
}
