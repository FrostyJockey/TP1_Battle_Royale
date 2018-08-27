using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawnerController : MonoBehaviour {

    //[SerializeField] private int spawnerNumber = 0;

    private bool isSpawnerAvailable;

    public bool IsSpawnerAvailable
    {
        get { return isSpawnerAvailable; }
        set
        {
            if (isSpawnerAvailable != value)
            {
                isSpawnerAvailable = value;

                if (isSpawnerAvailable == false)
                {
                    Debug.Log(gameObject.name + " is NOT available!");
                }
                else
                {
                    Debug.Log(gameObject.name + " is available!");
                }
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
