using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Transform _left;
    [SerializeField] private Transform _right;

    [SerializeField] private Transform _marker;

    public void UpdateProgress(float progress)
    {
        var x = _left.position.x + (_right.position.x - _left.position.x) * progress;
        _marker.transform.position = new Vector2(x, _marker.transform.position.y);
    }
}
