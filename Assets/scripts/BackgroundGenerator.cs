using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;

    void Start()
    {
        var player = GameObject.FindWithTag("player_ship");
        for (var i = 0; i < 10000; i++)
        {
            GameObject asteroid = new GameObject();
            SpriteRenderer sprite = asteroid.AddComponent<SpriteRenderer>();
            sprite.sprite = sprites[Random.Range(0, sprites.Length)];
            sprite.sortingLayerName = "background";
            sprite.color = new Color(1, 1, 1, 0.3f);
            asteroid.transform.Rotate(new Vector3(0, 0, Random.Range(0, 180)));
            asteroid.transform.position = new Vector3(
                player.transform.position.x + Random.Range(-1000, 1000),
                player.transform.position.y + Random.Range(-1000, 1000),
                0
            );
        }
    }

}
