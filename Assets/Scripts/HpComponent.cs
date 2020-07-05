using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpComponent : MonoBehaviour
{
    [SerializeField] public float _value;
    [SerializeField] public bool _heal;

    private bool _activated = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Player" || _activated)
            return;

        if (_heal)
            Character.Instance.OnHeal(_value);
        else
            Character.Instance.OnDamage(_value);

        _activated = true;
    }
}
