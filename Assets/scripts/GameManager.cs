using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    Camera camera;

    float desiredSize = 5;

    static Enemy[] enemies;

    static GameObject player;
    static Rigidbody2D playerBody;

    static GameObject youDied;

    public static GameObject getPlayer()
    {
        return player;
    }

    public static Rigidbody2D getPlayerBody()
    {
        return playerBody;
    }

    public static bool dead;

    void Start()
    {
        youDied = GameObject.FindWithTag("asdf1");
        youDied.SetActive(false);

        camera = FindObjectOfType<Camera>();
        player = GameObject.FindWithTag("player_ship");
        playerBody = player.GetComponent<Rigidbody2D>();

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

    public static void IgnoreCollisionsForEnemyBullet(Collider2D bulletCollider)
    {
        foreach (var enemy in enemies)
        {
            Physics2D.IgnoreCollision(bulletCollider, enemy.collider);
        }
    }

    public static void died()
    {
        dead = true;

        foreach ( var x in GameObject.FindGameObjectsWithTag("killable_ship_component"))
        {
            x.GetComponent<IdleAnimation>().pause();
        }

        youDied.SetActive(true);
    }
}

public class Enemy
{
    public GameObject go;
    public Collider2D collider;
    public CrabScript script;

    public Enemy(GameObject go)
    {
        this.go = go;
        this.collider = go.GetComponent<Collider2D>();
        this.script = go.GetComponent<CrabScript>();
    }
}
