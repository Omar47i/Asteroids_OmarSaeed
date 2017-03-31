using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour {

    [SerializeField]
    private Text currentScoreText;

    [SerializeField]
    private List<GameObject> m_Lives;

    private int m_ConsumedLives = 0;

    void Start()
    {
        // .. Listen to the score update events
        ScoreManager.Instance.m_CurrentScoreUpdatedEvent.AddListener(OnCurrentScoreUpdated);

        GameObject.FindGameObjectWithTag(Tags.LevelManager).GetComponent<PlayerLives>().m_LiveConsumedEvent.AddListener(OnLifeConsumed);
    }

    /// <summary>
    /// Update current score UI when its value changes
    /// </summary>
    private void OnCurrentScoreUpdated(int newScore)
    {
        currentScoreText.text = newScore.ToString();
    }

    /// <summary>
    /// Update lives UI (heart) when we consume it
    /// </summary>
    private void OnLifeConsumed()
    {
        m_Lives[m_ConsumedLives].SetActive(false);

        m_ConsumedLives++;
    }
}
