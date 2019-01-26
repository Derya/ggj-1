using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    [SerializeField]
    GameObject turretProjectile;

    [SerializeField]
    public GameObject flower;

    [SerializeField]
    Sprite[] idleSprites;

    [SerializeField]
    Sprite[] fireSprites;

    [SerializeField]
    float idleFrameTime;

    [SerializeField]
    float fireFrameTime;

    SpriteRenderer targetRenderer;

    public bool readyToFire = true;
    public bool queueFire = false;

    void Start()
    {
        targetRenderer = flower.GetComponent<SpriteRenderer>();
        StartCoroutine(playAnimation(idleSprites, idleFrameTime, 0));
    }

    void Update()
    {

    }

    public void fire()
    {
        if (!readyToFire)
        {
            queueFire = true;
        }
        else
        {
            StopAllCoroutines();
            readyToFire = false;
            StartCoroutine(playAnimation(fireSprites, fireFrameTime, 0));
        }
    }

    IEnumerator playAnimation(Sprite[] frames, float frameTime, int frame)
    {
        targetRenderer.sprite = frames[frame];

        yield return new WaitForSeconds(frameTime);

        if (frames == fireSprites && frame == 3)
        {
            spawnBullet();
        }

        if (frame + 1 == frames.Length)
        {
            if (queueFire)
            {
                queueFire = false;
                StartCoroutine(playAnimation(fireSprites, fireFrameTime, 0));
            }
            else
            {
                readyToFire = true;
                StartCoroutine(playAnimation(idleSprites, idleFrameTime, 0));
            }
        }
        else
        {
            StartCoroutine(playAnimation(frames, frameTime, frame + 1));
        }
    }

    public void spawnBullet()
    {
        Vector3 initialPosition = flower.transform.position;

        GameObject bullet = Instantiate(turretProjectile, initialPosition, flower.transform.rotation) as GameObject;
        bullet.GetComponent<Rigidbody2D>().AddForce(flower.transform.up * 1);
    }
}
