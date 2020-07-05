using System.Collections.Generic;
using Newtonsoft.Json;


public class LevelSettings
{
    [JsonProperty("duration")]
    public int Duration;

    [JsonProperty("startSpeed")]
    public float StartSpeed;

    [JsonProperty("endSpeed")]
    public float EndSpeed;

    [JsonProperty("characters")]
    public Dictionary<string, int> Characters;

    [JsonProperty("lut")]
    public Dictionary<string, int> Lut;
}

public class EndlessSettings
{
    [JsonProperty("characters")]
    public Dictionary<string, float[]> Characters;

    [JsonProperty("lut")]
    public Dictionary<string, int> Lut;
}
