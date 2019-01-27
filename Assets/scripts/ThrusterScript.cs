using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterScript : MonoBehaviour
{
    [SerializeField]
    ThrusterType type;

    [SerializeField]
    float flameSpawnTime;

    [SerializeField]
    GameObject exhaustPrefab;

    SpriteRenderer targetRenderer;

    private bool emitFlames = false;

    void Start()
    {

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
        yield return new WaitForSeconds(flameSpawnTime);

        if(emitFlames)
        {
            GameObject newExhaust = Instantiate(exhaustPrefab) as GameObject;
        }

        StartCoroutine(spawnFlame());
    }
}
