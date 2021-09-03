using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class AdsPlacement : MonoBehaviour, IUnityAdsListener
    {
        public UnityEvent OnFinish;
        public UnityEvent OnStart;

        [SerializeField] private string _id;

        private bool isReady;

        public void Start()
        {
            Advertisement.AddListener(this);
        }

        public void Show()
        {
            if (!isReady)
            {
                OnFinish?.Invoke();
                return;
            }

            Advertisement.Show(_id);
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            OnStart?.Invoke();
        }

        public void OnUnityAdsDidFinish(string id, ShowResult showResult)
        {
            if (id != _id)
                return;

            OnFinish?.Invoke();
        }

        public void OnUnityAdsReady(string id)
        {
            if (id == _id)
                isReady = true;
        }

        public void OnUnityAdsDidError(string message)
        {
            print("Ad Error: " + message);
        }
    }
}
