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

    GameObject player;

    bool disarmedEntirely = false;
    float countDownToRun = 0;

    bool colonized;

    Rigidbody2D body;

    private static int healthPerStage = 2;

    private float desiredDistance = 5;

    int stage = -1;
    int hitsThisStage = 0;

    void Start()
    {
        overlayRenderer = overlay.GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("player_ship");
        body = GetComponent<Rigidbody2D>();
        StartCoroutine(randomizeDesiredDistance());
    }

    IEnumerator randomizeDesiredDistance()
    {
        yield return new WaitForSeconds(1);

        desiredDistance = Random.Range(3, 5);

        StartCoroutine(randomizeDesiredDistance());
    }

    private void Update()
    {
        if (countDownToRun > 0)
        {
            countDownToRun -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!disarmedEntirely)
        {
            aggro_AI();
        } 
        else if (!colonized)
        {
            if (countDownToRun <= 0)
            {
                runAway_AI();
            }
            else
            {
                aggro_AI();
            }
        }
        else
        {
            body.drag = 0.5f;
            body.angularDrag = 0.5f;
        }
    }

    void runAway_AI()
    {
        body.angularDrag = 0.5f;

        Vector3 vectorAwayFromTarget = gameObject.transform.position - player.transform.position;
        float angleToTarget = Mathf.Atan2(vectorAwayFromTarget.y, vectorAwayFromTarget.x) * Mathf.Rad2Deg;

        Quaternion q = Quaternion.AngleAxis(angleToTarget - 90, Vector3.forward);
        float force;
        if (angleToTarget < 0)
        {
            force = Mathf.Lerp(3, 0, angleToTarget / 180f);
        }
        else
        {
            force = Mathf.Lerp(0, 3, angleToTarget / 180f);
        }

        gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, q, force);

        if (body.velocity.magnitude < 10)
        {
            body.AddForce(gameObject.transform.up * 1);
        }
        else
        {
            body.AddForce(gameObject.transform.up * -1);
        }
    }

    void aggro_AI()
    {
        Vector3 vectorToTarget = player.transform.position - gameObject.transform.position;
        float angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angleToTarget - 90, Vector3.forward);
        float force;
        if (angleToTarget < 0)
        {
            force = Mathf.Lerp(3, 0, angleToTarget / 180f);
        }
        else
        {
            force = Mathf.Lerp(0, 3, angleToTarget / 180f);
        }

        gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, q, force);

        if (vectorToTarget.magnitude > desiredDistance)
        {
            if (body.GetVector(vectorToTarget).magnitude < 20)
            {
                body.AddForce(gameObject.transform.up * 1);
            }
        }
        else
        {
            body.AddForce(gameObject.transform.up * -1);
        }
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
                if (ants.Length == 0)
                {
                    disarmedEntirely = true;
                    countDownToRun = 5f;
                }
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
            print("crab bonk!");
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
            gameObject.GetComponent<IdleAnimation>().pause();
        }
        overlayRenderer.sprite = overlayStages[stage];
    }

    public bool isColonized()
    {
        return colonized;
    }

}
