using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShipController : MonoBehaviour, IMovingObject
{
    [HideInInspector]                            // Raised when a hazard hit this object
    public ObjectHitEventBase m_ObjectHitEvent = new ObjectHitEventBase();

    private ShipMotor m_Motor;    // the driving motor of the player spaceship
    private ShipCannon m_Cannon;  // the cannon responsible for projectiles firing

    private MovingObjectHealth m_HPController;  // control player health and determine of the player should die

    [SerializeField]
    private int m_HP = 1;

    [SerializeField]
    private float m_MovementSpeed;     // forward/backward movement speed

    [SerializeField]
    private float m_TurnSpeed;         // rotation speed in degrees

    [SerializeField]
    private float m_ProjectileSpeed;   // movement speed of the projectile

    [SerializeField]
    private float m_ProjectileCoolDownDur;   // time between each projectile fire

    [SerializeField]                   
    private GameObject m_ProjectilePrefab;

    [SerializeField]                  // the position of firing the projectiles
    private List<Transform> m_CannonTransforms;

    // .. Cach frequently used variables
    private Transform tr;
    private Rigidbody2D rb;

    void Awake()
    {
        tr = transform;
        rb = GetComponent<Rigidbody2D>();

        m_Motor = new ShipMotor(rb, tr, m_MovementSpeed, m_TurnSpeed);
        m_Cannon = new ShipCannon(CannonType.Single, m_ProjectilePrefab, m_ProjectileSpeed, m_ProjectileCoolDownDur);
        m_HPController = new MovingObjectHealth(m_HP, m_ObjectHitEvent);
    }

    /// <summary>
    /// Called from the PlayerShipUpgrader.cs to upgrade the ship cannons
    /// </summary>
    public void UpgradeShipCannon()
    {
        m_Cannon.UpgradeCannonType();
    }

    /// <summary>
    /// Fire a projectile at the cannon world position
    /// </summary>
    public void Fire()
    {
        m_Cannon.Fire(tr.up, m_CannonTransforms);
    }

    /// <summary>
    /// Move the ship to the forward or backward
    /// </summary>
    /// <param name="dir">direction of movement</param>
    public void Move(float dir)
    {
        m_Motor.Move(dir);
    }

    /// <summary>
    /// Turn the ship to the left or right
    /// </summary>
    /// <param name="dir">direction of turning</param>
    public void Turn(float dir)
    {
        m_Motor.Turn(dir);
    }

    void Update()
    {
        // .. Call the cannon update function to update frame-based variables like cooldown counter
        m_Cannon.Update();
    }

    void FixedUpdate()
    {
        // .. Physics movement and rotation must take place here in FixedUpdate
        m_Motor.FixedUpdate();
    }

    /// <summary>
    /// Not implemented for player spaceship
    /// </summary>
    /// <param name="dir"></param>
    public void Move(Vector3 dir)
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
