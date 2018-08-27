﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTemp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.up * 0.2f);
        else if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * 0.2f);
        else if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.down * 0.2f);
        else if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * 0.2f);
    }
}