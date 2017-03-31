using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    public TeleporterType type;

    void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 playerShip = other.transform.position;

        // .. Teleport the player to the other side of the screen
        if (type == TeleporterType.Up)
        {
            other.transform.position = new Vector3(playerShip.x, -playerShip.y + 10f, playerShip.z);
        }
        else if (type == TeleporterType.Down)
        {
            other.transform.position = new Vector3(playerShip.x, -playerShip.y - 10f, playerShip.z);
        }
        else if (type == TeleporterType.Right)
        {
            other.transform.position = new Vector3(-playerShip.x + 10f, playerShip.y, playerShip.z);
        }
        else if (type == TeleporterType.Left)
        {
            other.transform.position = new Vector3(-playerShip.x - 10f, playerShip.y, playerShip.z);
        }
    }
}

public enum TeleporterType
{
    Up,
    Down,
    Left,
    Right
}
