using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    private static int healthPerStage = 3;

    [SerializeField]
    GameObject mainSprite;
    SpriteRenderer targetRenderer;

    [SerializeField]
    Sprite[] stages;

    int stage = 0;
    int hitsThisStage = 0;

    bool colonized;

    // Start is called before the first frame update
    void Start()
    {
        targetRenderer = mainSprite.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player_bullet")
        {
            hitsThisStage = hitsThisStage + 1;
            if (hitsThisStage > healthPerStage)
            {
                hitsThisStage = 0;
                nextStage();
            }
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == "player_ship")
        {
            print("bonk!");
        }
    }

    void nextStage()
    {
        if (stage == stages.Length - 1)
        {
            return;
        }

        stage = stage + 1;
        if (stage == stages.Length - 1)
        {
            colonized = true;
        }
        targetRenderer.sprite = stages[stage];
    }

    public bool isColonized()
    {
        return colonized;
    }

}
