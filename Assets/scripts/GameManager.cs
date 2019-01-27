using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    Camera camera;

    float desiredSize = 5;

    GameObject[] enemies;

    GameObject player;

    void Start()
    {
        camera = FindObjectOfType<Camera>();
        enemies = GameObject.FindGameObjectsWithTag("enemy_ship");
        player = GameObject.FindWithTag("player_ship");
    }

    // Update is called once per frame
    void Update()
    {
        float closestEnemy = distToClosestEnemy();

        desiredSize = Mathf.Clamp(closestEnemy + 2, 7, 20);

        float diff = Mathf.Abs(camera.orthographicSize - desiredSize);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, desiredSize, diff * Time.deltaTime / 5);
    }

    float distToClosestEnemy()
    {
        float ret = 999;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, player.transform.position);
            if (dist < ret)
            {
                ret = dist;
            }
        }

        return ret;
    }
}
