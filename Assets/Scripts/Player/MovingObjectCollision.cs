using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovingObjectCollision : MonoBehaviour
{
    // .. player spaceship will not take hits if it is invulnerable
    public bool m_Invulnerable
    {
        get
        {
            return m_invulnerable;
        }
        set
        {
            if (value)
                PlayerInvulnerableEvent.Invoke(2f);  // Make player invulnerable for 2 seconds

            m_invulnerable = value;
        }
    }

    private bool m_invulnerable = false;

    [HideInInspector]                                // Invoked when a flicker is about to happen
    public PlayerInvulnerableEventBase PlayerInvulnerableEvent = new PlayerInvulnerableEventBase();

    public MovingObjecType m_Type;              // Type of the moving object: -Player spaceship, -Asteroid, -Enemy spaceship, -Peojectile

    public GameObject m_ExplosionPrefab;        // explosion prefab that will be created when object's HP is <=0     

    void Start()
    {
        // .. Subscribe to the hit event for every moving object since all moving objects implements this event
        if (m_Type != MovingObjecType.CannonUpgradePickup)
            GetComponent<IMovingObject>().OnObjectHit().AddListener(OnObjectHit);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // .. Handle each case of collision for moving objects
        switch (m_Type)
        {
            case MovingObjecType.PlayerShip:
                {
                    if (m_Invulnerable || other.collider.tag == Tags.CannonUpgradePickup)
                        return;
                    // .. Inflict damage to player if he hit any moving object
                    GetComponent<IMovingObject>().GetMovingObjectHealth().InflictDamage();
                }
                break;

            case MovingObjecType.Asteroid:
                {
                    // .. Inflict damage to the asteroid if he hit only a projectile
                    if (other.collider.tag == Tags.PlayerProjectile || other.collider.tag == Tags.EnemyProjectile)
                    {
                        GetComponent<IMovingObject>().GetMovingObjectHealth().InflictDamage();
                    }
                }
                break;

            case MovingObjecType.EnemyShip:
                {
                    // .. Inflict damage to the enemy ship if hit by a projectile or an asteroid
                    if (other.collider.tag == Tags.PlayerProjectile || other.collider.tag == Tags.EnemyProjectile
                        || other.collider.tag == Tags.Asteroid)
                    {
                        GetComponent<IMovingObject>().GetMovingObjectHealth().InflictDamage();
                    }
                }
                break;

            case MovingObjecType.PlayerProjectile:
                {
                    // .. Inflict damage to the projectile if hit by any moving object except projectiles
                    if (other.collider.tag == Tags.Asteroid ||  other.collider.tag == Tags.EnemyShip)
                    {
                        // .. Update the score on hitting this object
                        if (other.collider.tag == Tags.Asteroid)
                        {
                            ScoreManager.Instance.m_ChangeCurrentScoreEvent.Invoke(ScoreAddition.Asteroid, transform.position);
                        }
                        else if (other.collider.tag == Tags.EnemyShip)
                        {
                            ScoreManager.Instance.m_ChangeCurrentScoreEvent.Invoke(ScoreAddition.EnemyShip, transform.position);
                        }

                        GetComponent<IMovingObject>().GetMovingObjectHealth().InflictDamage();
                    }
                }
                break;
            case MovingObjecType.EnemyProjectile:
                {
                    // .. Inflict damage to the projectile if hit by any moving object except projectiles
                    GetComponent<IMovingObject>().GetMovingObjectHealth().InflictDamage();
                }
                break;
            case MovingObjecType.CannonUpgradePickup:
                {
                    // .. Inform the player that this pickup has been consumed
                    GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<PlayerShipUpgrader>().m_ShipUpgradeEvent.Invoke();

                    // .. Destroy the pickup
                    Destroy(gameObject);
                }
                break;
        }
    }

    /// <summary>
    /// Handle moving object hit for each moving object
    /// </summary>
    /// <param name="newHP"></param>
    private void OnObjectHit(int newHP)
    {
        if (newHP <= 0)
        {
            if (m_Type == MovingObjecType.PlayerShip || m_Type == MovingObjecType.EnemyShip || m_Type == MovingObjecType.Asteroid)
            {
                // .. Play sound effect
                SoundManager.Instance.PlaySoundEffect(SoundEffectName.EXPLODE, true);
            }

            Explode();
        }
    }

    /// <summary>
    /// Explode on hitting a spaceship
    /// </summary>
    public void Explode()
    {
        // .. Create explosion at the moving object position
        if (m_ExplosionPrefab != null)
            Instantiate(m_ExplosionPrefab, transform.position, Quaternion.identity);

        if (m_Type == MovingObjecType.PlayerShip)
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }
}

public enum MovingObjecType
{
    PlayerShip,
    EnemyShip,
    Asteroid,
    PlayerProjectile,
    EnemyProjectile,
    CannonUpgradePickup,
}

public class PlayerInvulnerableEventBase : UnityEvent<float>
{ }