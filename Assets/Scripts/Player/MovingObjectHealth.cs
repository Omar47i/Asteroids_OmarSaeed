using UnityEngine.Events;

/// <summary>
/// Add this component to every moving object to enable its hit points
/// </summary>
public class MovingObjectHealth
{
    private int m_HP;                    // this moving object's hit points, every fire results in a decrease in the object's HP by one

    private ObjectHitEventBase m_ObjectHitEvent; // Raised when the object is being hit

    public MovingObjectHealth() { }

    public MovingObjectHealth(int hp, ObjectHitEventBase objectHitEvent)
    {
        m_HP = hp;
        m_ObjectHitEvent = objectHitEvent;
    }

    /// <summary>
    /// inflict an amount of damage from the object's health
    /// </summary>
    /// <param name="amount">amount of damage inflicted</param>
    public void InflictDamage(int amount = 1)
    {
        m_HP -= amount;

        m_ObjectHitEvent.Invoke(m_HP);
    }

    /// <summary>
    /// Add amount of health to the object's HP
    /// </summary>
    /// <param name="amount">amount of added HP</param>
    public void AddHealth(int amount = 1)
    {
        m_HP += amount;
    }
}
