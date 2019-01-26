using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{

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
            new TurretWrapper(turretLeftBot, -70, 160), 
            new TurretWrapper(turretRightBot, 20, -110),
            new TurretWrapper(turretLeftTop, 70, -160),
            new TurretWrapper(turretRightTop, 110, -20)
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
        Vector3 vectorShipForward = transform.up;

        float angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        float shipAngle = Mathf.Atan2(vectorShipForward.y, vectorShipForward.x) * Mathf.Rad2Deg;

        wrapper.restrictAngle(angleToTarget, shipAngle);

        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        Quaternion newRotation = Quaternion.RotateTowards(turret.transform.rotation, q,  turretRotationFactor);
        bool allowFire = newRotation.Equals(turret.transform.rotation);
        turret.transform.rotation = newRotation;

        if (allowFire)
        {
            print("fire!");
        }
    }

}

class TurretWrapper
{
    public GameObject turret;
    private float angle1Raw;
    private float angle2Raw;

    public TurretWrapper(GameObject turret, float angle1Raw, float angle2Raw)
    {
        this.turret = turret;
        this.angle1Raw = angle1Raw;
        this.angle2Raw = angle2Raw;
        //this.smallestAngle = Mathf.Abs(Mathf.DeltaAngle(angle1, angle2));
    }

    public float restrictAngle(float angleToTarget, float shipAngle)
    {
        float angle1 = angle1Raw;
        float angle2 = angle2Raw;

        float angle1Delta = Mathf.Abs(Mathf.DeltaAngle(angle, angle1));
        float angle2Delta = Mathf.Abs(Mathf.DeltaAngle(angle, angle2));

        if ((angle1Delta + angle2Delta) <= smallestAngle)
        {
            return angle;
        }
        else if (angle1Delta < angle2Delta)
        {
            return angle1;
        }
        else
        {
            return angle2;
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
