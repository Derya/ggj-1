using UnityEngine;
using System.Collections;

public class IdleAnimation : MonoBehaviour
{
    SpriteRenderer targetRenderer;

    [SerializeField]
    Sprite[] frames;

    [SerializeField]
    float frameTime;

    void Start()
    {
        targetRenderer = GetComponent<SpriteRenderer>();
        beginIdleAnim();
    }

    void Update()
    {

    }

    void beginIdleAnim()
    {
        StartCoroutine(playAnimation(0));
    }

    IEnumerator playAnimation(int frame)
    {
        targetRenderer.sprite = frames[frame];

        yield return new WaitForSeconds(frameTime);

        if (frame + 1 == frames.Length)
        {
            beginIdleAnim();
        }
        else
        {
            StartCoroutine(playAnimation(frame + 1));
        }
    }
}
