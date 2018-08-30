using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathGifAnimator : MonoBehaviour {
    Texture[] frames = new Texture[] { };
    int framesPerSecond = 10;

    private void Update()
    {
        int index = (int)(Time.time * framesPerSecond) % frames.Length;
        GetComponent<Renderer>().material.mainTexture = frames[index];
    }
}
