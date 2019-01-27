using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    Camera camera;

    float desiredSize = 5;

    Enemy[] enemies;

    GameObject player;

    void Start()
    {
        camera = FindObjectOfType<Camera>();
        player = GameObject.FindWithTag("player_ship");

        GameObject[] asdf = GameObject.FindGameObjectsWithTag("enemy_ship");
        enemies = new Enemy[asdf.Length];
        for (var i = 0; i < enemies.Length; i++)
        {
            enemies[i] = new Enemy(asdf[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float closestEnemy = distToClosestEnemy();

        if (closestEnemy < 25)
        {
            desiredSize = Mathf.Clamp(closestEnemy + 2, 5, 20);
        }
        else
        {
            desiredSize = 5;
        }

        float diff = Mathf.Abs(camera.orthographicSize - desiredSize);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, desiredSize, diff * Time.deltaTime / 5);
    }

    float distToClosestEnemy()
    {
        float ret = 999;

        foreach (var enemy in enemies)
        {
            if (!enemy.script.isColonized())
            {
                float dist = Vector3.Distance(enemy.go.transform.position, player.transform.position);
                if (dist < ret)
                {
                    ret = dist;
                }
            }
        }

        return ret;
    }
}

public class Enemy
{
    public GameObject go;
    public CrabScript script;

    public Enemy(GameObject go)
    {
        this.go = go;
        this.script = go.GetComponent<CrabScript>();
    }
}
