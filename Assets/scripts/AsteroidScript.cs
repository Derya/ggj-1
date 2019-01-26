using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    [SerializeField]
    GameObject mainSprite;
    SpriteRenderer targetRenderer;

    [SerializeField]
    Sprite[] stages;

    int stage = 0;

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
        Destroy(collision.gameObject);
    }

    void strike()
    {
        if (stage == stages.Length - 1)
        {
            return;
        }

        stage = stage + 1;
        targetRenderer.sprite = stages[stage];
    }
}
