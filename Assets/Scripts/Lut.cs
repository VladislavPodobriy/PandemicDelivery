using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LutType  
{
    Mask,
    Vaccine,
    Paper
}

public class Lut : MonoBehaviour
{
    [SerializeField] private LutType _type;

    private bool _activated;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Player" || _activated)
            return;

        if (_type == LutType.Mask)
            LutController.AddMask();
        if (_type == LutType.Vaccine)
            LutController.AddVaccine();

        Destroy(gameObject);

        _activated = true;
    }
}
