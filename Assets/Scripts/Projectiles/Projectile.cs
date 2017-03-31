using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IMovingObject
{
    [SerializeField]
    private int m_HP = 1;

    private Rigidbody2D rb;           // reference to the projectile's rigid body

    private float m_Speed;            // speed of the moving projectile
    private Vector2 m_Direction;      // direction of movement

    private bool initialized = false; // don't give commands to the projectile if it's not initialized

    // .. Declare the commands that the AI could give to the projectile
    private ICommand moveCommand;

    [HideInInspector]                            // Raised when a hazard hit this object
    public ObjectHitEventBase m_ObjectHitEvent = new ObjectHitEventBase();

    [HideInInspector]
    public MovingObjectHealth m_HPController;    // hit points controller for this projectile

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        m_Speed = 0f;
        m_Direction = Vector2.zero;

        moveCommand = new MoveAndTurnCommand();
        m_HPController = new MovingObjectHealth(m_HP, m_ObjectHitEvent);
    }

    public void SetProjectile(float sp, Vector2 dir)
    {
        m_Speed = sp;
        m_Direction = dir;

        // .. Set the initialized flag to true in order for the projectile to start moving
        initialized = true;
    }

    void Update()
    {
        if (!initialized)
            return;

        moveCommand.Execute(gameObject);
    }

    public void Move(float dir)
    {
        Vector2 movementDir = (m_Speed * m_Direction) * Time.deltaTime;

        rb.position += movementDir;
    }

    public void Move(Vector3 dir)
    {
        throw new NotImplementedException();
    }

    public void Turn(float dir)
    {
        throw new NotImplementedException();
    }

    public ObjectHitEventBase OnObjectHit()
    {
        return m_ObjectHitEvent;
    }

    public MovingObjectHealth GetMovingObjectHealth()
    {
        return m_HPController;
    }
}
