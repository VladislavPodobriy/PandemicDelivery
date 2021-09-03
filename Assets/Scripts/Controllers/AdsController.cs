using System;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Assets.Scripts
{
    public class AdsController: Singleton<AdsController>
    {
        static string gameId = "3629877";
        static bool testMode = false;

        public static int N = 0;

        [SerializeField] private AdsPlacement _video;

        public void Start()
        {
            Advertisement.Initialize(gameId, testMode);
        }

        public static void ShowVideo()
        {
            Instance._video.Show();
        }

        public static void OnVideoEnd(Action callback)
        {
            Instance._video.OnFinish.AddListener(()=>callback());
        }
    }
}
