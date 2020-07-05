using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveingComponent : MonoBehaviour
{
    [SerializeField] private float _speed;

    void FixedUpdate()
    {
        transform.position += new Vector3(-_speed, 0);
    }
}
