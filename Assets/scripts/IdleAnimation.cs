using UnityEngine;
using System.Collections;

public class IdleAnimation : MonoBehaviour
{
    SpriteRenderer targetRenderer;

    [SerializeField]
    Sprite[] frames;

    [SerializeField]
    float frameTime;

    private int frameNumber = 0;

    private bool paused = false;

    void Start()
    {
        targetRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(playAnimation());
    }

    IEnumerator playAnimation()
    {
        targetRenderer.sprite = frames[frameNumber];

        yield return new WaitForSeconds(frameTime);

        if (!paused)
        {
            frameNumber = frameNumber + 1;
            if (frameNumber == frames.Length)
            {
                frameNumber = 0;
            }
        }

        StartCoroutine(playAnimation());
    }

    public void pause()
    {
        paused = true;
    }

    public void unpause()
    {
        paused = false;
    }
}
