using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pixelplacement;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerDataManager : Singleton<PlayerDataManager>
    {
        public static PlayerData PlayerData;
        public static int EndlessScore;

        public static void InitGame()
        {
            if (PlayerPrefs.HasKey("data"))
            {
                LoadData();
                if (PlayerData.Hp <= 0)
                    ResetProgress();
            }
            else
                ResetProgress();
        }

        public static void ResetProgress()
        {
            PlayerData = new PlayerData
            {
                Progress = 0,
                OrdersList = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8}
                    .OrderBy(e => Random.Range(0, 100))
                    .Append(9)
                    .ToList(),
                Hp = 3,
                MedicineKit = 5
            };
            SaveProgress();
        }

        public static void SaveProgress()
        {
            var json = JsonConvert.SerializeObject(PlayerData);
            PlayerPrefs.SetString("data", json);
            PlayerPrefs.SetInt("score", EndlessScore);
        }
    
        private static void LoadData()
        {
            var dataJson = PlayerPrefs.GetString("data");
            Debug.Log(dataJson);
            PlayerData = JsonConvert.DeserializeObject<PlayerData>(dataJson);
            Debug.Log(PlayerData.OrdersList[0]);
            EndlessScore = PlayerPrefs.GetInt("score");
        }
    }
}
