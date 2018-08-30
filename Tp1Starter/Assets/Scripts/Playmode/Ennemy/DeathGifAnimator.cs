using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 *
 * Trouvé sur https://forum.unity.com/threads/animated-gifs.10126/
 * 
 */
public class DeathGifAnimator : MonoBehaviour
{
    private bool isOver;
    public Sprite[] frames;
    public SpriteRenderer explosion;
    public float frameRate = 0.1f;

    private int currentImage;
    // Use this for initialization
    void Start()
    {
        isOver = false;
        currentImage = 0;
        InvokeRepeating("ChangeImage", 0.1f, frameRate);
    }

    private void ChangeImage()
    {
        if (!isOver)
        {
            if (currentImage < frames.Length - 1)
            {
                if (currentImage == frames.Length - 1)
                {
                    CancelInvoke("ChangeImage");
                    Destroy(explosion);
                }
                currentImage += 1;
                explosion.sprite = frames[currentImage];
            }
            else
            {
                GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
                isOver = true;
            }
        }
    }
}