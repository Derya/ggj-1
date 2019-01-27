using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    bool initialized = false;

    [SerializeField]
    GameObject turtleImage;

    [SerializeField]
    GameObject asteroidImage;

    [SerializeField]
    GameObject crabImage;

    GameObject player;

    RectTransform my_own_damn_self;

    void Start()
    {
        my_own_damn_self = GetComponent<RectTransform>();
    }

    void initialize()
    {
        Asteroid[] asteroids = GameManager.getAsteroids();
        Enemy[] crabs = GameManager.getEnemies();
        player = GameManager.getPlayer();
        RectTransform playerIconRT = GameObject.Find("map_turtle_image_prefab").GetComponent<RectTransform>();

        float factor = 300 / 200;

        foreach (var asteroid in asteroids)
        {
            GameObject x = Instantiate(asteroidImage) as GameObject;
            asteroid.rt = x.GetComponent<RectTransform>();

            asteroid.rt.SetParent(my_own_damn_self);

            float worldRightOfPlayer = asteroid.go.transform.position.x - player.transform.position.x;
            float worldUpOfPlayer = asteroid.go.transform.position.y - player.transform.position.y;

            asteroid.rt.anchoredPosition = new Vector2(
                worldRightOfPlayer * factor,
                worldUpOfPlayer * factor
            );
        }

        foreach (var crab in crabs)
        {
            GameObject x = Instantiate(crabImage) as GameObject;
            crab.rt = x.GetComponent<RectTransform>();

            crab.rt.SetParent(my_own_damn_self);

            float worldRightOfPlayer = crab.go.transform.position.x - player.transform.position.x;
            float worldUpOfPlayer = crab.go.transform.position.y - player.transform.position.y;

            crab.rt.anchoredPosition = new Vector2(
                worldRightOfPlayer * factor,
                worldUpOfPlayer * factor
            );
        }
    }

    void Update()
    {
        if (!initialized)
        {
            if (GameManager.getTime() > 2)
            {
                initialize();
                initialized = true;
            }
            return;
        }

        Asteroid[] asteroids = GameManager.getAsteroids();
        Enemy[] crabs = GameManager.getEnemies();
        player = GameManager.getPlayer();
        RectTransform playerIconRT = GameObject.Find("map_turtle_image_prefab").GetComponent<RectTransform>();

        playerIconRT.rotation = player.transform.rotation;

        float factor = 300 / (GameManager.range * 1.5f);

        foreach (var asteroid in asteroids)
        {
            float worldRightOfPlayer = asteroid.go.transform.position.x - player.transform.position.x;
            float worldUpOfPlayer = asteroid.go.transform.position.y - player.transform.position.y;

            asteroid.rt.anchoredPosition = new Vector2(
                worldRightOfPlayer * factor,
                worldUpOfPlayer * factor
            );
        }

        foreach (var crab in crabs)
        {
            float worldRightOfPlayer = crab.go.transform.position.x - player.transform.position.x;
            float worldUpOfPlayer = crab.go.transform.position.y - player.transform.position.y;

            crab.rt.anchoredPosition = new Vector2(
                worldRightOfPlayer * factor,
                worldUpOfPlayer * factor
            );
        }
    }
}
