using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMotor
{
    // Spaceship attributes
    private Rigidbody2D rb;
    private Transform tr;
    private float m_MovementSpeed;
    private float m_TurnSpeed;
    private float m_Damping;

    // Flags to inform the fixed update if there is a movement or rotation
    private bool m_Turn;
    private bool m_Move;

    private Vector2 m_MovementDirection;
    private float m_RotationAngle;

    public bool m_ActivateLookAtBehaviour = false;  // Will look at an object if set to true

    private Quaternion m_LookQuat;            // look rotaion to a position (player)
    public ShipMotor(Rigidbody2D rigidBody, Transform trans, float movementSp, float turnSp)
    {
        // .. Intialize ship motor values
        rb = rigidBody;
        tr = trans;
        m_MovementSpeed = movementSp;
        m_TurnSpeed = turnSp;
    }

    public void Move(float dir)
    {
        m_Move = true;

        m_MovementDirection = (m_MovementSpeed * dir * tr.up) * Time.deltaTime;
    }

    public void Move(Vector3 dir)
    {
        m_Move = true;

        m_MovementDirection = (m_MovementSpeed * dir) * Time.deltaTime;
    }

    public void Turn(float dir)
    {
        m_Turn = true;

        m_RotationAngle = -dir * m_TurnSpeed;
    }

    public void LookAtPosition(Vector3 pos)
    {
        m_Turn = true;

        CalculateLookRotation(pos);
    }

    private void CalculateLookRotation(Vector3 playerPos)
    {
        Vector3 lookDirection = playerPos - tr.position;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        m_LookQuat = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    public void FixedUpdate()
    {
        if (m_Move)
        {
            rb.position += m_MovementDirection;

            m_Move = false;
        }

        if (m_Turn)
        {
            if (m_ActivateLookAtBehaviour)
            {
                tr.rotation = Quaternion.Slerp(tr.rotation, m_LookQuat, Time.deltaTime * 5f);
            }
            else
            {
                tr.Rotate(0f, 0f, m_RotationAngle * Time.deltaTime);
            }

            m_Turn = false;
        }
    }
}
