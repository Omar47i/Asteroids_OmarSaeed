using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeScoreEventBase : UnityEvent<ScoreAddition, Vector3>
{ }

public class CurrentScoreUpdatedEventBase : UnityEvent<int>
{ }

public class ScoreManager : MonoBehaviour {

    public static ScoreManager Instance;           // make me singleton!

    [HideInInspector]                              // to be fired when current score is updated
    public CurrentScoreUpdatedEventBase m_CurrentScoreUpdatedEvent = new CurrentScoreUpdatedEventBase(); 

    [HideInInspector]                              // to be fired when we did something like firing on an asteroid and want to change the score
    public ChangeScoreEventBase m_ChangeCurrentScoreEvent = new ChangeScoreEventBase();   

    [HideInInspector]
    public UnityEvent m_BestScoreUpdatedEvent;     // to be fired when best score updates

    [SerializeField]
    private GameObject[] m_ScorePrefabs;           // create a scoring text based on the added score

    private bool m_DisplayNewHighScore = false;    // display new high score text only one time 

    public int CurrentScore
    {
        get
        {
            return currentScore;
        }
        set
        {
            currentScore = value;

            m_CurrentScoreUpdatedEvent.Invoke(currentScore);      // fire current score updated event here
        }
    }

    public int BestScore
    {
        set
        {
            bestScore = value;

            m_BestScoreUpdatedEvent.Invoke();         // fire best score updated event here

            PlayerSettings.SetBestScore(bestScore);   // save the best score in playerprefs
        }
        get
        {
            return bestScore;
        }
    }

    private int currentScore;                // current user score
    private int bestScore;                   // fetched from PlayerPrefs if exists

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
            Destroy(gameObject);

        m_BestScoreUpdatedEvent = new UnityEvent();

        InitScoreManager();
    }

    void InitScoreManager()
    {
        // .. Initialize score mamanger values
        currentScore = 0;

        // .. Get the last saved best score
        bestScore = PlayerSettings.GetBestScore();
 
        // .. Listen to the gameOver event to update the best score
        GameManager.Instance.m_GameOverEvent.AddListener(OnGameOver);

        m_ChangeCurrentScoreEvent.AddListener(UpdateScore);
    }

    /// <summary>
    /// Update user's score, Invoked outside this class
    /// </summary>
    /// <param name="type">type of the score as each type has a value</param>
    private void UpdateScore(ScoreAddition type, Vector3 scoreTextPosition)
    {
        CurrentScore += (int)type;

        // .. Display a new high score text if we got a new high score
        if (CurrentScore > BestScore && !m_DisplayNewHighScore && BestScore != 0)
        {
            m_DisplayNewHighScore = true;

            Instantiate(ScoringTextResolution(0), new Vector3(scoreTextPosition.x, scoreTextPosition.y, 0f), Quaternion.identity);

            // .. Play sfx
            SoundManager.Instance.PlaySoundEffect(SoundEffectName.NEW_HIGH_SCORE, false, 1f);
        }
        // .. Create a floating score text based on the score addition value
        else
        {
            Instantiate(ScoringTextResolution((int)type), new Vector3(scoreTextPosition.x, scoreTextPosition.y, 0), Quaternion.identity);
        }
    }

    /// <summary>
    /// Invoked by the GameOverEvent, used to update best score
    /// </summary>
    private void OnGameOver()
    {
        // .. We have a new best score
        if (currentScore > bestScore)
        {
            // .. Hide the current score and display only the best score
            BestScore = CurrentScore;
        }
    }

    /// <summary>
    /// Get the correspnding prefab based on the score
    /// </summary>
    /// <param name="score">score added</param>
    /// <returns>the corresponding score text prefab</returns>
    private GameObject ScoringTextResolution(int score)
    {
        GameObject scorePrefab = null;

        switch(score)
        {
            case 5:
                scorePrefab = m_ScorePrefabs[0];
                break;
            case 10:
                scorePrefab = m_ScorePrefabs[1];
                break;
            case 20:
                scorePrefab = m_ScorePrefabs[2];
                break;
            case 50:
                scorePrefab = m_ScorePrefabs[3];
                break;
            case 0:
                scorePrefab = m_ScorePrefabs[4];
                break;
        }

        return scorePrefab;
    }
}

public enum ScoreAddition
{
    Asteroid = 10,
    EnemyShip = 20,
    CannonUpgradePickup = 10,
}