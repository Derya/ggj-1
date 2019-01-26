using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    [SerializeField]
    GameObject turretProjectile;

    [SerializeField]
    float turretFiringArcSize;

    [SerializeField]
    float shipRotationFactor;

    [SerializeField]
    float turretRotationFactor;

    [SerializeField]
    GameObject turretLeftBot;
    [SerializeField]
    GameObject turretRightBot;
    [SerializeField]
    GameObject turretLeftTop;
    [SerializeField]
    GameObject turretRightTop;

    TurretWrapper[] turrets;

    Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        turrets = new TurretWrapper[4] { 
            new TurretWrapper(turretLeftBot, -70),//-70, 160), 
            new TurretWrapper(turretRightBot, 20),//20, -110),
            new TurretWrapper(turretLeftTop, 70),//70, -160),
            new TurretWrapper(turretRightTop, 110),//110, -20)
        };
    }


    void Update()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        foreach (TurretWrapper turret in turrets)
        {
            handleTurret(mouse, turret);
        }
    }

    void FixedUpdate()
    {
        handleThrusters();

    }

    void handleThrusters()
    {
        bool holdingLeft = Input.GetKey(App.ROTATE_LEFT_KEY);
        bool holdingRight = Input.GetKey(App.ROTATE_RIGHT_KEY);
        bool holdingGas = Input.GetKey(App.ACCELERATE_KEY);
        bool holdingBrake = Input.GetKey(App.DECERATE_KEY);

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
        if (holdingGas && !holdingBrake)
        {
            gas = Gas.forward;
        }
        else if (holdingBrake && !holdingGas)
        {
            gas = Gas.back;
        }

        if (direction == Direction.left)
        {
            body.AddTorque(shipRotationFactor);
        }
        else if (direction == Direction.right)
        {
            body.AddTorque(-shipRotationFactor);
        }

        if (gas == Gas.forward)
        {
            body.AddRelativeForce(Vector2.up);
        }
        else if (gas == Gas.back)
        {
            body.AddRelativeForce(Vector2.down);
        }
    }

    void handleTurret(Vector3 target, TurretWrapper wrapper)
    {
        GameObject turret = wrapper.turret;
        Vector3 vectorToTarget = target - turret.transform.position;

        float angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angleToTarget - 90, Vector3.forward);
        Quaternion newRotation = Quaternion.RotateTowards(turret.transform.rotation, q, turretRotationFactor);
        bool canFire = Quaternion.Angle(newRotation, turret.transform.rotation) < 2;
        turret.transform.rotation = newRotation;
        wrapper.clamp();

        if (canFire && Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(turretProjectile, turret.transform.position, turret.transform.rotation) as GameObject;
            bullet.GetComponent<Rigidbody2D>().AddForce(turret.transform.up * 100);
        }
    }

    public class TurretWrapper
    {
        public GameObject turret;
        public TurretScript script;
        public float arcCenter;

        public TurretWrapper(GameObject turret, float arcCenter)
        {
            this.turret = turret;
            this.arcCenter = arcCenter;
            this.script = turret.GetComponent<TurretScript>();
        }

        public void clamp()
        {

        }
    }

}

enum Direction
{
    left, right, none
}

enum Gas
{
    forward, back, none
}
