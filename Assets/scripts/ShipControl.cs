﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    [SerializeField]
    float thrustFactor;

    [SerializeField]
    float turretRotationFactor;

    [SerializeField]
    float maxAngularVelocity;

    [SerializeField]
    GameObject firePrefab;

    [SerializeField]
    GameObject turretLeftBot;
    [SerializeField]
    GameObject turretRightBot;
    [SerializeField]
    GameObject turretLeftTop;
    [SerializeField]
    GameObject turretRightTop;

    [SerializeField]
    GameObject[] thrusterRefs;

    ThrusterWrapper[] thrusters;

    TurretWrapper[] turrets;

    Rigidbody2D body;

    int health;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        turrets = new TurretWrapper[4] { 
            new TurretWrapper(turretLeftBot, turretLeftBot.transform.localEulerAngles),
            new TurretWrapper(turretRightBot, turretRightBot.transform.localEulerAngles),
            new TurretWrapper(turretLeftTop, turretLeftTop.transform.localEulerAngles),
            new TurretWrapper(turretRightTop, turretRightTop.transform.localEulerAngles),
        };

        thrusters = new ThrusterWrapper[thrusterRefs.Length];

        for (var i = 0; i < thrusterRefs.Length; i++)
        {
            thrusters[i] = new ThrusterWrapper(thrusterRefs[i]);
        }

        health = 10;
    }

    void Update()
    {
        if (health > 0)
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            foreach (TurretWrapper turret in turrets)
            {
                handleTurret(mouse, turret);
            }
        }
    }

    void FixedUpdate()
    {
        if (health > 0) 
        {
            handleControls();
        }
    }

    void handleControls()
    {
        bool holdingLeft = Input.GetKey(App.ROTATE_LEFT_KEY);
        bool holdingRight = Input.GetKey(App.ROTATE_RIGHT_KEY);
        bool holdingGas = Input.GetKey(App.ACCELERATE_KEY);
        bool holdingReverse = Input.GetKey(App.DECERATE_KEY);

        Direction direction = Direction.none;
        if (holdingLeft && !holdingRight)
        {
            direction = Direction.left;
        }
        else if (holdingRight && !holdingLeft)
        {
            direction = Direction.right;
        }

        Gas gas = Gas.none;
        if (holdingGas && !holdingReverse)
        {
            gas = Gas.forward;
        }
        else if (holdingReverse && !holdingGas)
        {
            gas = Gas.back;
        }

        if (direction == Direction.left)
        {
            body.AddTorque(Mathf.Clamp(15 * (maxAngularVelocity - body.angularVelocity), 0, 15));
        }
        else if (direction == Direction.right)
        {
            body.AddTorque(Mathf.Clamp(15 * (-maxAngularVelocity - body.angularVelocity), -15, 0));
        }

        if (gas == Gas.forward)
        {
            float forwardVel = transform.InverseTransformDirection(body.velocity).y;
            body.AddRelativeForce(Vector2.up * thrustFactor * Mathf.Lerp(1, 5, -forwardVel / 2.0f));
        }
        else if (gas == Gas.back)
        {
            float forwardVel = transform.InverseTransformDirection(body.velocity).y;
            body.AddRelativeForce(Vector2.up * -thrustFactor * Mathf.Lerp(1, 5, forwardVel / 2.0f));
        }

        if (Input.GetKey(App.BRAKE_KEY))
        {
            body.drag = 0.8f;
            body.angularDrag = 0.8f;
        }
        else
        {
            body.drag = 0f;
            body.angularDrag = 0f;
        }

        foreach (var thruster in thrusters)
        {
            thruster.script.setEmitFlames(gas, direction);
        }
    }

    void handleTurret(Vector3 target, TurretWrapper turretWrapper)
    {
        GameObject turret = turretWrapper.gameObject;
        Vector3 vectorToTarget = target - turret.transform.position;

        float angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angleToTarget - 90, Vector3.forward);
        Quaternion newRotation = Quaternion.RotateTowards(turret.transform.rotation, q, turretRotationFactor);
        bool canFire = Quaternion.Angle(newRotation, turret.transform.rotation) < 2;
        turret.transform.rotation = newRotation;
        turretWrapper.clamp();

        if (canFire && Input.GetMouseButtonDown(0))
        {
            turretWrapper.script.fire();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy_bullet")
        {
            Destroy(collision.gameObject);

            takeDamage();
        }
    }

    void takeDamage()
    {
        health--;

        
        GameObject newFire = Instantiate(
            firePrefab
        ) as GameObject;

        newFire.transform.RotateAroundLocal(Vector3.forward, Random.Range(-50, 50));

        newFire.transform.parent = transform;
        newFire.transform.localPosition = new Vector3(Random.Range(-5, 5), Random.Range(-7, 7), -5);

        if (health <= 0)
        {
            GameManager.died();
        }
    }

}

public class TurretWrapper
{
    public GameObject gameObject;
    public TurretScript script;
    public float arcCenter;
    static readonly float maxAngle = 65;

    public TurretWrapper(GameObject gameObject, Vector3 localRot)
    {
        this.gameObject = gameObject;
        this.arcCenter = localRot.z;
        this.script = gameObject.GetComponent<TurretScript>();
    }

    public void clamp()
    {
        Vector3 localRot = gameObject.transform.localEulerAngles;

        if (Mathf.Abs(Mathf.DeltaAngle(localRot.z, arcCenter)) > maxAngle)
        {
            float edge1 = arcCenter + maxAngle;
            float edge2 = arcCenter - maxAngle;

            if (Mathf.Abs(Mathf.DeltaAngle(localRot.z, edge1)) < Mathf.Abs(Mathf.DeltaAngle(localRot.z, edge2)))
            {
                localRot.z = edge1;
            }
            else
            {
                localRot.z = edge2;
            }
            gameObject.transform.localEulerAngles = localRot;
        }

    }
}

public class ThrusterWrapper
{
    public GameObject gameObject;
    public ThrusterScript script;

    public ThrusterWrapper(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.script = gameObject.GetComponent<ThrusterScript>();
    }
}
