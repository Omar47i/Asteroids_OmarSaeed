using System.Collections.Generic;
using UnityEngine;

public class ShipCannon
{
    private CannonType m_CannonType;  // the type of firing cannon (single or double projectiles)

    private GameObject m_ProjectilePrefab;  // the projectile prefab

    private Vector2 m_FireDir;        // direction of the fired projectile

    private float m_Speed;            // speed of the projectile

    private float m_CooldownDur;      // duration between each fire
    private float m_CooldownCounter;

    public ShipCannon(CannonType cannonType, GameObject projectilePrefab, float speed, float coolDownDur)
    {
        m_CannonType = cannonType;
        m_ProjectilePrefab = projectilePrefab;
        m_Speed = speed;
        m_CooldownDur = coolDownDur;

        m_CooldownCounter = m_CooldownDur;
    }

    /// <summary>
    /// Fires a projectile based on the cannon type
    /// </summary>
    public void Fire(Vector2 dir, List<Transform> worldPositionTrans)
    {
        // .. If our cannon still in cooling down phase, return
        if (m_CooldownCounter > 0)
            return;

        // .. Update the cool down counter
        m_CooldownCounter = m_CooldownDur;

        if (m_CannonType == CannonType.Single)
        {
            // .. Instanitate the projectile at the cannon position
            GameObject proj = GameObject.Instantiate
                (m_ProjectilePrefab, worldPositionTrans[0].position, Quaternion.identity);

            // .. Set the projectile stats
            proj.GetComponent<Projectile>().SetProjectile(m_Speed, dir);
        }

        else if (m_CannonType == CannonType.Double)
        {
            // .. Instanitate three projectiles
            for (int i = 0; i < 3; i++)
            {
                // .. Instanitate the projectile at the cannon position
                GameObject proj = GameObject.Instantiate
                    (m_ProjectilePrefab, worldPositionTrans[i].position, Quaternion.identity);

                // .. Set the projectile stats
                proj.GetComponent<Projectile>().SetProjectile(m_Speed, dir);
            }
        }

        // .. Play sound effect
        SoundManager.Instance.PlaySoundEffect(SoundEffectName.FIRE_PROJECTILE, true);
    }

    /// <summary>
    /// Upgrades cannon type to fire double projectiles at once
    /// </summary>
    public void UpgradeCannonType()
    {
        m_CannonType = CannonType.Double;
    }

    /// <summary>
    /// Update fire intervals counter every frame
    /// </summary>
    public void Update()
    {
        m_CooldownCounter -= Time.deltaTime;
    }
}

// every space ship can fire single or double projectiles based on their cannon type
public enum CannonType
{
    Single,
    Double,
}