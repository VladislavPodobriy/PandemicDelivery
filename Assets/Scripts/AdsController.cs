using Pixelplacement;
using UnityEngine.Advertisements;

public class AdsController: Singleton<AdsController>
{
    static string gameId = "3629877";
    static bool testMode = false;

    public static int N = 0;

    public AdsPlacement Video;

    public void Start()
    {
        Advertisement.Initialize(gameId, testMode);
    }

    public static void ShowVideo()
    {
        Instance.Video.Show();
    }
}
