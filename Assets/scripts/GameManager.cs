using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Camera camera;

    float desiredSize = 5;

    static Enemy[] enemies;

    static Asteroid[] asteroids;

    public static Enemy[] getEnemies()
    {
        return enemies;
    }

    public static Asteroid[] getAsteroids()
    {
        return asteroids;
    }

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

    static float time = 0;

    public static float getTime()
    {
        return time;
    }

    public static bool dead;

    void Start()
    {

        generateAsteroids();
        generateEnemies();

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

        asdf = GameObject.FindGameObjectsWithTag("asteroid");
        asteroids = new Asteroid[asdf.Length];
        for (var i = 0; i < asteroids.Length; i++)
        {
            asteroids[i] = new Asteroid(asdf[i]);
        }
    }


    [SerializeField]
    GameObject crabPrefab;

    [SerializeField]
    GameObject[] asteroidPrefabs;

    static readonly public int lesser = 2;
    static readonly public int density = 12 / lesser;
    static readonly public int range = 200 / lesser;

    void generateEnemies()
    {
        for (var i = 0; i < density; i++)
        {
            GameObject newCrab = Instantiate(crabPrefab) as GameObject;

            newCrab.transform.Rotate(new Vector3(0, 0, Random.Range(0, 180)));
            newCrab.transform.position = new Vector3(
                Random.Range(-range, range),
                Random.Range(-range, range),
                0
            );
        }
    }

    void generateAsteroids()
    {
        for (var i = 0; i < density * 3; i++)
        {
            GameObject newAsteroid = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)]) as GameObject;

            newAsteroid.transform.Rotate(new Vector3(0, 0, Random.Range(0, 180)));
            newAsteroid.transform.position = new Vector3(
                Random.Range(-range, range),
                Random.Range(-range, range),
                0
            );
        }

    }

    void Update()
    {
        time += Time.deltaTime;

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

        bool win = true;

        foreach (var asteroid in asteroids)
        {
            //if (!asteroid.script.isColonized())
            //{
            //    win = false;
            //    break;
            //}
        }

        foreach (var enemy in enemies)
        {
            if (!enemy.script.isDisarmedCompletely())
            {
                win = false;
                break;
            }
        }

        if (win && time > 5)
        {
            SceneManager.LoadScene("ending");
        }
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
    public bool colonized;
    public RectTransform rt;

    public Enemy(GameObject go)
    {
        this.go = go;
        this.collider = go.GetComponent<Collider2D>();
        this.script = go.GetComponent<CrabScript>();
    }
}

public class Asteroid
{
    public GameObject go;
    public AsteroidScript script;
    public bool colonized;
    public RectTransform rt;

    public Asteroid(GameObject go)
    {
        this.go = go;
        this.script = go.GetComponent<AsteroidScript>();
    }
}
