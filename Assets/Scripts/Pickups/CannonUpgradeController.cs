using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonUpgradeController : MonoBehaviour, IMovingObject
{
    [HideInInspector]                            // Raised when a hazard hit this object
    public ObjectHitEventBase m_ObjectHitEvent = new ObjectHitEventBase();

    [SerializeField]
    private float m_MovementSpeed;      // Pickup movement speed

    [HideInInspector]
    public Quaternion m_StartingAngle;  // starting angle of movement

    private ShipMotor m_Motor;    // Use the same motor of the spaceship since they move and rotate

    // .. Cach frequently used variables
    private Transform tr;
    private Rigidbody2D rb;
    private bool init = false;

    // .. Define the commands that can be issued by the asteroid AI
    private ICommand moveTurnCommand;

    void Awake()
    {
        // .. Initialize the commands that the AI can issue to the asteroid
        moveTurnCommand = new MoveAndTurnCommand();

        tr = transform;
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetPickup(float movementSpeed, Quaternion angle)
    {
        m_MovementSpeed = movementSpeed;
        m_StartingAngle = angle;

        m_Motor = new ShipMotor(rb, tr, m_MovementSpeed, 0f);
        init = true;
    }

    public MovingObjectHealth GetMovingObjectHealth()
    {
        throw new NotImplementedException();
    }

    public void Move(Vector3 dir)
    {
        m_Motor.Move(dir);
    }

    public void Move(float dir)
    {
        throw new NotImplementedException();
    }

    public ObjectHitEventBase OnObjectHit()
    {
        return null;
    }

    public void Turn(float dir)
    {
        throw new NotImplementedException();
    }

    void FixedUpdate()
    {
        if (init)
            m_Motor.FixedUpdate();
    }

    //@Debug
    void Update()
    {
        if (init)
            moveTurnCommand.Execute(gameObject);
    }
}
