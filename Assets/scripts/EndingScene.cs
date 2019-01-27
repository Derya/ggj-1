using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour
{
    SpriteRenderer targetRenderer;

    [SerializeField]
    Sprite[] frames;

    [SerializeField]
    float frameTime;

    private bool ready = false;

    private int frameNumber = 0;

    private bool paused = false;

    void Start()
    {
        targetRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(playAnimation());
    }

    void Update()
    {
        if (ready && Input.anyKeyDown)
        {
            SceneManager.LoadScene("opening");
        }
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
                frameNumber = frameNumber - 1;
                ready = true;
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
