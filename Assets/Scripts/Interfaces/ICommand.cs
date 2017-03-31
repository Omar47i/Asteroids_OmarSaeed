using System;
using UnityEngine;

/// <summary>
/// All commands issued by the player and the AI must implement this interface
/// </summary>
public interface ICommand
{
    /// <summary>
    /// The execute function to be implemented by player/AI on the actor
    /// </summary>
    /// <param name="actor"></param>
    void Execute(GameObject actor = null);
}

public class NullCommand : ICommand
{
    public void Execute(GameObject actor = null)
    {
        // .. intentially leave it blank to avoid raising a null reference (Null Object Design Pattern)
    }
}

/// 
/// <summary>
/// Command usage: Move any moving object in forward/backward direction
/// </summary>
public class MoveAndTurnCommand : ICommand
{
    public void Execute(GameObject actor = null)
    {
        // .. Get the controller of the actor
        IMovingObject controller = actor.GetComponent<IMovingObject>();

        // .. Execute the movement and rotation commands for the actor
        if (actor.tag == Tags.Player)
        {
            InputHandler inputHandler = actor.GetComponent<InputHandler>(); 

            controller.Move(inputHandler.m_Vertical);
            controller.Turn(inputHandler.m_Horizontal);
        }

        else if (actor.tag == Tags.Asteroid)
        {
            AsteroidController asteroid = actor.GetComponent<AsteroidController>();

            Vector3 v = asteroid.m_StartingAngle * Vector3.up;
            controller.Move(v);
            controller.Turn(1f);
        }

        else if (actor.tag == Tags.EnemyShip)
        {
            EnemyShipController enemyShip = actor.GetComponent<EnemyShipController>();

            Vector3 v = enemyShip.m_StartingAngle * Vector3.up;
            controller.Move(v);
            controller.Turn(1f);
        }

        else if (actor.tag == Tags.PlayerProjectile || actor.tag == Tags.EnemyProjectile || actor.tag == Tags.EnemyShip)
        {
            controller.Move(1f);
        }

        else if (actor.tag == Tags.CannonUpgradePickup)
        {
            CannonUpgradeController cannonController = actor.GetComponent<CannonUpgradeController>();

            Vector3 v = cannonController.m_StartingAngle * Vector3.up;
            controller.Move(v);
            //controller.Turn(1f);
        }
    }
}

/// <summary>
/// Command usage: Fire a projectile
/// </summary>
public class FireCommand : ICommand
{
    public void Execute(GameObject actor = null)
    {
        // .. Execute the fire commands for the player ship
        if (actor.tag == Tags.Player)
        {
            PlayerShipController controller = actor.GetComponent<PlayerShipController>();

            controller.Fire();
        }

        // .. Execute the fire commands for the enemy ship
        else if (actor.tag == Tags.EnemyShip)
        {
            EnemyShipController controller = actor.GetComponent<EnemyShipController>();

            controller.Fire();
        }
    }
}