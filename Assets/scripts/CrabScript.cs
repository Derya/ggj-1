using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabScript : MonoBehaviour
{
    [SerializeField]
    GameObject[] ants;

    [SerializeField]
    GameObject overlay;
    SpriteRenderer overlayRenderer;

    [SerializeField]
    Sprite[] overlayStages;

    bool colonized;

    private static int healthPerStage = 2;

    int stage = -1;
    int hitsThisStage = 0;

    // Start is called before the first frame update
    void Start()
    {
        overlayRenderer = overlay.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player_bullet")
        {
            Destroy(collision.gameObject);

            if (ants.Length > 0)
            {
                int idx = Random.Range(0, ants.Length);
                Destroy(ants[idx]);
                ants = ants.removeAt(idx);
            }
            else
            {
                hitsThisStage = hitsThisStage + 1;
                if (hitsThisStage > healthPerStage)
                {
                    hitsThisStage = 0;
                    nextStage();
                }
            }
        }

        else if (collision.gameObject.tag == "player_ship")
        {
            print("bonk!");
        }
    }

    void nextStage()
    {
        if (stage == overlayStages.Length - 1)
        {
            return;
        }

        stage = stage + 1;
        if (stage == overlayStages.Length - 1)
        {
            colonized = true;
        }
        overlayRenderer.sprite = overlayStages[stage];
    }

    public bool isColonized()
    {
        return colonized;
    }

}
