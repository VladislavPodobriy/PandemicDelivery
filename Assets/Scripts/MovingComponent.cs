using UnityEngine;

public class MovingComponent : MonoBehaviour
{
    [SerializeField] private float _speed;

    void FixedUpdate()
    {
        transform.position += new Vector3(-_speed, 0);
    }
}
