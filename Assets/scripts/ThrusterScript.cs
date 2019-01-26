using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterScript : MonoBehaviour
{
    [SerializeField]
    GameObject mainSprite;

    [SerializeField]
    ThrusterType type;

    [SerializeField]
    Sprite[] idleSprites;

    [SerializeField]
    float idleFrameTime;

    [SerializeField]
    float flameSpawnTime;

    SpriteRenderer targetRenderer;

    private bool emitFlames = false;

    void Start()
    {
        targetRenderer = mainSprite.GetComponent<SpriteRenderer>();
        beginIdleAnim();
    }

    void Update()
    {

    }

    public void setEmitFlames(Gas gas, Direction direction)
    {
        switch (type)
        {
            case ThrusterType.forward:
                emitFlames = gas == Gas.forward;
                break;

            case ThrusterType.backward:
                emitFlames = gas == Gas.back;
                break;

            case ThrusterType.left_wing_forward:
                emitFlames = direction == Direction.left;
                break;

            case ThrusterType.left_wing_backward:
                emitFlames = direction == Direction.right;
                break;

            case ThrusterType.right_wing_forward:
                emitFlames = direction == Direction.right;
                break;

            case ThrusterType.right_wing_backward:
                emitFlames = direction == Direction.left;
                break;
        }
    }

    IEnumerator spawnFlame()
    {
        yield return new WaitForSeconds(flameSpawnTime * Random.Range(0.9f, 1.1f));

        if(emitFlames)
        {
            print("fwoosh");
        }

        StartCoroutine(spawnFlame());
    }

    void beginIdleAnim()
    {
        StartCoroutine(playAnimation(idleSprites, idleFrameTime, 0));
    }

    IEnumerator playAnimation(Sprite[] frames, float frameTime, int frame)
    {
        targetRenderer.sprite = frames[frame];

        yield return new WaitForSeconds(frameTime);

        if (frame + 1 == frames.Length)
        {
            beginIdleAnim();
        }
        else
        {
            StartCoroutine(playAnimation(frames, frameTime, frame + 1));
        }
    }
}
