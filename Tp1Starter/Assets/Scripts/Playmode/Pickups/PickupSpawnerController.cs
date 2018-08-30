using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawnerController : MonoBehaviour {

    private bool isSpawnerAvailable;

    public bool IsSpawnerAvailable
    {
        get { return isSpawnerAvailable; }
        set
        {
            if (isSpawnerAvailable != value)
            {
                isSpawnerAvailable = value;
            }
        }
    }

    private void Awake()
    {
        isSpawnerAvailable = true;
    }

    private void OnDisable()
    {
        IsSpawnerAvailable = false;
    }
}
