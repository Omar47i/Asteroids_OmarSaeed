using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Auther : Omar Saeed
/// Simulate the Input.GetAxis("") function using UI button events
/// </summary>
public class MobileInputHandler : MonoBehaviour
{
    [HideInInspector]
    public float m_VerticalAxis = 0f;

    [HideInInspector]
    public float m_HorizontalAxis = 0f;

    float m_LerpSpeed = 10f;

    // .. Used only to update lerping values for axises
    bool m_VerticalAxisDown = false;
    bool m_HorizontalAxisRightDown = false;
    bool m_HorizontalAxisLeftDown = false;

    [HideInInspector]
    public bool m_FireDown = false;

    void Start()
    {
        // .. Hide controls on game over
        GameManager.Instance.m_GameOverEvent.AddListener(OnGameOver);
    }

    private void OnGameOver()
    {
        foreach(Transform trans in transform)
        {
            trans.gameObject.SetActive(false);
        }
    }

    public void OnFireDown()
    {
        m_FireDown = true;
    }

    public void OnFireUp()
    {
        m_FireDown = false;
    }

    public void OnMoveDown()
    {
        m_VerticalAxisDown = true;
    }

    public void OnMoveUp()
    {
        m_VerticalAxisDown = false;
    }

    public void OnTurnRightDown()
    {
        m_HorizontalAxisRightDown = true;
    }

    public void OnTurnRightUp()
    {
        m_HorizontalAxisRightDown = false;
    }

    public void OnTurnLeftDown()
    {
        m_HorizontalAxisLeftDown = true;
    }

    public void OnTurnLeftUp()
    {
        m_HorizontalAxisLeftDown = false;
    }

    void Update()
    {
        // .. Lerp if it's down
        if (m_VerticalAxisDown)
        {
            m_VerticalAxis = Mathf.Lerp(m_VerticalAxis, 1f, Time.deltaTime * m_LerpSpeed);

            CheckReachOne(ref m_VerticalAxis);

        }
        else if (!m_VerticalAxisDown)
        {
            m_VerticalAxis = Mathf.Lerp(m_VerticalAxis, 0f, Time.deltaTime * m_LerpSpeed);

            CheckReturnToZeroValue(ref m_VerticalAxis);
        }

        // .. Check right direction
        if (m_HorizontalAxisRightDown)
        {
            m_HorizontalAxis = Mathf.Lerp(m_HorizontalAxis, 1f, Time.deltaTime * m_LerpSpeed);

            CheckReachOne(ref m_HorizontalAxis);
        }

        // .. check left direction
        else if (m_HorizontalAxisLeftDown)
        {
            m_HorizontalAxis = Mathf.Lerp(m_HorizontalAxis, -1f, Time.deltaTime * m_LerpSpeed);

            CheckReachMinusOne(ref m_HorizontalAxis);
        }

        else if (!m_HorizontalAxisLeftDown)
        {
            m_HorizontalAxis = Mathf.Lerp(m_HorizontalAxis, 0f, Time.deltaTime * m_LerpSpeed);

            CheckReturnToZeroValue(ref m_HorizontalAxis);
        }
    }

    void CheckReturnToZeroValue(ref float axis)
    {
        if (Mathf.Abs(axis) <= 0.01f)
        {
            axis = 0f;
        }
    }

    void CheckReachOne(ref float axis)
    {
        if (Mathf.Abs(axis) >= 0.99f)
        {
            axis = 1f;
        }
    }

    void CheckReachMinusOne(ref float axis)
    {
        if (Mathf.Abs(axis) >= 0.99f)
        {
            axis = -1f;
        }
    }
}