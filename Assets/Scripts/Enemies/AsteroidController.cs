using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AsteroidController : MonoBehaviour, IMovingObject
{
    [HideInInspector]                            // Raised when a hazard hit this object
    public ObjectHitEventBase m_ObjectHitEvent = new ObjectHitEventBase();

    [HideInInspector]
    public MovingObjectHealth m_HPController;    // hit points controller for this asteroid

    [HideInInspector]
    public Quaternion m_StartingAngle; // starting angle of movement

    [SerializeField]
    private float m_MovementSpeed;     // forward/backward movement speed

    [SerializeField]
    private float m_TurningSpeed;      // rotation speed in degrees

    [SerializeField]
    private int m_HP;                  // hit points for this asteroid

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

    /// <summary>
    /// Set the asteroid stats
    /// </summary>
    /// <param name="movementSpeed">Asteroid movement speed</param>
    /// <param name="turningSpeed">Asteroid rotatiob speed</param>
    /// <param name="angle">Starting angle</param>
    /// <param name="HP">Hitpoints</param>
    /// <param name="explosion">The explosion prefab assigned to this asteroid</param>
    public void SetAsteroid(float movementSpeed, float turningSpeed, Quaternion angle, int HP, GameObject explosion = null)
    {
        m_MovementSpeed = movementSpeed;
        m_TurningSpeed = turningSpeed;
        m_StartingAngle = angle;
        m_HP = HP;

        m_HPController = new MovingObjectHealth(m_HP, m_ObjectHitEvent);
        m_Motor = new ShipMotor(rb, tr, m_MovementSpeed, m_TurningSpeed);

        // .. Assign the explosion prefab based on the asteroid size
        if (explosion != null)
        {
            GetComponent<MovingObjectCollision>().m_ExplosionPrefab = explosion;
        }

        init = true;              // Now that the asteroid is initialized and well set, Start moving it
    }

    /// <summary>
    /// Move the asteroid with a specific direction regradless of its forward direction
    /// </summary>
    /// <param name="dir">direction of movement</param>
    public void Move(Vector3 dir)
    {
        m_Motor.Move(dir);
    }

    /// <summary>
    /// Not implemented for asteroids
    /// </summary>
    /// <param name="dir"></param>
    public void Move(float dir)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Turn the ship to the left or right
    /// </summary>
    /// <param name="dir">direction of turning</param>
    public void Turn(float dir)
    {
        m_Motor.Turn(dir);
    }

    void FixedUpdate()
    {
        if (init)
            m_Motor.FixedUpdate();
    }

    void Update()
    {
        if (init)
            moveTurnCommand.Execute(gameObject);
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

public class ObjectHitEventBase : UnityEvent<int>
{
}