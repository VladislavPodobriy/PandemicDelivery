using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

public class AdsPlacement : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] public UnityEvent onFinish;
    [SerializeField] public UnityEvent onStart;

    [SerializeField] private string placementId;

    private bool ready;

    public void Start()
    {
        Advertisement.AddListener(this);
    }

    public void Show()
    {
        if (!ready)
        {
            onFinish?.Invoke();
            return;
        }

        Advertisement.Show(placementId);
    }

    public void OnUnityAdsDidFinish(string id, ShowResult showResult)
    {
        if (id != placementId)
            return;

        if (showResult == ShowResult.Finished)
        {
            print("Ad Finished");
        }
        else if (showResult == ShowResult.Skipped)
        {
            print("Ad Skipped");
        }
        else if (showResult == ShowResult.Failed)
        {
            print("Ad Failed");
        }

        onFinish?.Invoke();
    }

    public void OnUnityAdsReady(string id)
    {
        print("Ready");
        print(id);
        if (id == placementId)
            ready = true;
    }

    public void OnUnityAdsDidError(string message)
    {
        print("Ad Error: " + message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        onStart?.Invoke();
    }
}
