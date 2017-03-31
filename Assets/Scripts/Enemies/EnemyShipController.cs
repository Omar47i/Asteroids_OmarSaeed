using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipController : MonoBehaviour, IMovingObject
{
    [HideInInspector]                            // Raised when a hazard hit this object
    public ObjectHitEventBase m_ObjectHitEvent = new ObjectHitEventBase();

    [HideInInspector]
    public MovingObjectHealth m_HPController;    // hit points controller for this asteroid

    [SerializeField]
    private float m_MovementSpeed;     // forward/backward movement speed

    [HideInInspector]
    public Quaternion m_StartingAngle; // starting angle of movement

    [SerializeField]
    private int m_HP;                  // hit points for this asteroid

    [SerializeField]                  // the position of firing the projectiles
    private List<Transform> m_CannonTransforms;

    [SerializeField]
    private float m_ProjectileSpeed;   // movement speed of the projectile

    [SerializeField]
    private float m_ProjectileCoolDownDur;   // time between each projectile fire

    [SerializeField]
    private GameObject m_ProjectilePrefab;

    private Transform m_PlayerShip;    // reference to the player's ship to be able to rotate towards it
    private ShipMotor m_Motor;         // Use the same motor of the spaceship since they move and rotate
    private ShipCannon m_Cannon;       // the cannon responsible for projectiles firing

    // .. Cach frequently used variables
    private Transform tr;
    private Rigidbody2D rb;
    private bool init = false;

    // .. Define the commands that can be issued by the asteroid AI
    private ICommand moveTurnCommand;
    private ICommand fireCommand;
    private bool fire = false;       // don't make the ship fire automatically once it's created

    void Awake()
    {
        // .. Initialize the commands that the AI can issue to the ship
        moveTurnCommand = new MoveAndTurnCommand();
        fireCommand = new FireCommand();

        tr = transform;
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Set the enemy ship stats
    /// </summary>
    /// <param name="movementSpeed">speed of movement</param>
    /// <param name="startingAngle"></param>
    /// <param name="HP">every enemy ship has one HP</param>
    /// <param name="playerShip">reference to the player ship to look at always</param>
    public void SetShip(float movementSpeed, Quaternion startingAngle, int HP, Transform playerShip)
    {
        m_PlayerShip = playerShip;
        m_MovementSpeed = movementSpeed;
        m_StartingAngle = startingAngle;
        m_HP = HP;

        // .. Create a new instance of HP controller to manage the enemy ship health
        m_HPController = new MovingObjectHealth(m_HP, m_ObjectHitEvent);

        // .. Create a motor for this enemy ship, the same motor used for player ship and asteroids
        m_Motor = new ShipMotor(rb, tr, m_MovementSpeed, 0f);

        // .. Create a cannon for firing projectiles at the player
        m_Cannon = new ShipCannon(CannonType.Single, m_ProjectilePrefab, m_ProjectileSpeed, m_ProjectileCoolDownDur);

        // .. Enable this enemy to look at the player
        m_Motor.m_ActivateLookAtBehaviour = true;          

        // Listen to the player hit event because we don't want to follow a dead player!
        m_PlayerShip.GetComponent<PlayerShipController>().m_ObjectHitEvent.AddListener(OnPlayerShipExploded);

        // .. Fire after a while
        StartCoroutine(StartFiringAfter(UnityEngine.Random.Range(.1f, .25f)));

        init = true;              // Now that the asteroid is initialized and well set, Start moving it
    }

    /// <summary>
    /// Fired when the player ship explodes
    /// </summary>
    /// <param name="newHP">new HP after hit, probably zero</param>
    private void OnPlayerShipExploded(int newHP)
    {
        if (newHP == 0)
        {
            init = false;  // to indicate that the player ship has been exploded
        }
    }

    /// <summary>
    /// Move the ship with a specific direction regradless of its forward direction
    /// </summary>
    /// <param name="dir">direction of movement</param>
    public void Move(Vector3 dir)
    {
        m_Motor.Move(dir);
    }

    /// <summary>
    /// Turn the ship to face the player 
    /// </summary>
    /// <param name="dir">neglected</param>
    public void Turn(float dir)
    {
        if (init)
            m_Motor.LookAtPosition(m_PlayerShip.position);
    }

    /// <summary>
    /// Fire a projectile at the cannon world position
    /// </summary>
    public void Fire()
    {
        m_Cannon.Fire(tr.up, m_CannonTransforms);
    }

    private IEnumerator StartFiringAfter(float dur)
    {
        // Play sound effect on enemy enter game area
        if (UnityEngine.Random.Range(0, 101) < 75)
        {
            // .. Play sound effect
            SoundManager.Instance.PlaySoundEffect(SoundEffectName.ENEMY_SPACESHIP_APPEAR);
        }

        yield return new WaitForSeconds(dur);

        fire = true;
    }

    public ObjectHitEventBase OnObjectHit()
    {
        return m_ObjectHitEvent;
    }

    public MovingObjectHealth GetMovingObjectHealth()
    {
        return m_HPController;
    }

    public void Move(float dir)
    {
        throw new NotImplementedException();
    }

    void FixedUpdate()
    {
        m_Motor.FixedUpdate();
    }

    void Update()
    {
        moveTurnCommand.Execute(gameObject);

        if (fire)
            fireCommand.Execute(gameObject);

        // .. Call the cannon update function to update frame-based variables like cooldown counter
        m_Cannon.Update();
    }
}
