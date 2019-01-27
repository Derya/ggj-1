using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterExhaustScript : MonoBehaviour
{
    [SerializeField]
    GameObject my_own_damn_self;

    SpriteRenderer spriteRenderer;

    private float timer;

    void Start()
    {
        timer = 0;

        spriteRenderer = my_own_damn_self.GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        timer += Time.deltaTime;

        // spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, timer / 6.0f));

        if (timer > 6)
        {
            Destroy(my_own_damn_self);
        }
    }
}
