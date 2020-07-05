using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareButton : MonoBehaviour
{
    private Button _button;

    public void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => { ScreenshotHelper.SaveAndShare(1400, 700, 24, Camera.main); });
    }
}
