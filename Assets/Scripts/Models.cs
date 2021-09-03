using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelObjectModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    
        [JsonProperty("chance")]
        public int Chance { get; set; }
    
        [JsonProperty("minSpeed")]
        public float MinimalSpeed { get; set; }
    }

    public class LutObjectModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    
        [JsonProperty("chance")]
        public int Chance { get; set; }
    }

    public class DataModel
    {
        [JsonProperty("endless")]
        public LevelModel Endless { get; set; }
    
        [JsonProperty("levels")]
        public List<LevelModel> Levels { get; set; }
    }

    public class LevelModel
    {
        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("startSpeed")]
        public float StartSpeed{ get; set; }

        [JsonProperty("endSpeed")]
        public float EndSpeed { get; set; }

        [JsonProperty("levelObjects")]
        public List<LevelObjectModel> LevelObjects { get; set; }

        [JsonProperty("lutObjects")]
        public List<LutObjectModel> LutObjects { get; set; }
    }

    public class GameItem
    {
        public Transform Transform;
        public string Name;
    }

    public enum LutType  
    {
        Mask,
        Vaccine,
        Paper
    }

    public class PlayerData
    {
        public int Progress { get; set; }

        public List<int> OrdersList { get; set; }
        
        public int Hp { get; set; }

        public int MedicineKit { get; set; }
        public int Vaccine { get; set; }
        public int Mask { get; set; }
    }
}