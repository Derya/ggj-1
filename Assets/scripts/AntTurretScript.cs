using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntTurretScript : MonoBehaviour
{
    [SerializeField]
    GameObject fireballPrefab;

    Rigidbody2D crabBody;

    float fireDelay;

    void Start()
    {
        crabBody = GetComponentInParent<Rigidbody2D>();
        fireDelay = Random.Range(1, 12);
    }

    void Update()
    {
        GameObject player = GameManager.getPlayer();
        float dist = Vector2.Distance(player.transform.position, transform.position);

        fireDelay -= Time.deltaTime;
        bool fireNow = false;
        if (fireDelay <= 0)
        {
            fireNow = true;
            fireDelay = Random.Range(1, 12);
        }

        if (fireNow && dist <= 20)
        {
            fire();
        }

        if (dist >= 25)
        {
            return;
        }

        //float timeEstimate = dist * projectileSpeed;

        //print(timeEstimate);
        //print(GameManager.getPlayerBody().velocity);

        //Vector2 target = ((Vector2) player.transform.position) + (GameManager.getPlayerBody().velocity * timeEstimate);

        Vector3 targetPos = player.transform.position;
        Vector3 thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    void fire()
    {
        Vector3 initialPosition = transform.position;

        GameObject bullet = Instantiate(fireballPrefab, initialPosition, transform.rotation) as GameObject;

        GameManager.IgnoreCollisionsForEnemyBullet(bullet.GetComponent<Collider2D>());

        Vector3 force = transform.up * 5;

        var bulletBody = bullet.GetComponent<Rigidbody2D>();

        bulletBody.velocity = crabBody.velocity;

        bulletBody.AddForce(force);
        //shipBody.AddForceAtPosition(-force, bullet.transform.position);
    }
}
