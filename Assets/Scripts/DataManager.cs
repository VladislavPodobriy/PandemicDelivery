using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pixelplacement;
using UnityEngine;

public class Data
{
    public int Progress = 0;

    public List<int> OrdersList = new List<int> {0,1,2,3,4,5,6,7,8}
        .OrderBy(e => Random.Range(0,100))
        .Append(9)
        .ToList();

    public int GameInitialized = 0;
    public int Hp = 3;

    public int MedicineKit = 5;
    public int Vaccine = 0;
    public int Mask = 0;
}


public class DataManager : Singleton<DataManager>
{
    public static Data Data;
    public static Data Endless;

    public static int Score;

    public static void InitGame()
    {
        if (PlayerPrefs.HasKey("data"))
        {
            LoadProgress();

            if (Data.Hp <= 0)
                ResetProgress();
        }
        else
            ResetProgress();
        
        Endless = new Data();
    }

    public static void ResetProgress()
    {
        Data = new Data();
        SaveProgress();
    }

    public static void LoadProgress()
    {
        var json = PlayerPrefs.GetString("data");
        Score = PlayerPrefs.GetInt("score");

        Data = JsonConvert.DeserializeObject<Data>(json);
    }

    public static void SaveProgress()
    {
        var json = JsonConvert.SerializeObject(Data);
        PlayerPrefs.SetString("data", json);
        PlayerPrefs.SetInt("score", Score);
    }
}
