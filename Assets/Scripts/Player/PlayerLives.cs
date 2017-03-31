using System.Collections;
using UnityEngine.Events;
using UnityEngine;

public class PlayerLives : MonoBehaviour {

    [HideInInspector]
    public int m_LivesCount = 3;                        // Total number of player lives

    [HideInInspector]                                   // Invoked when player life is consumed
    public UnityEvent m_LiveConsumedEvent = new UnityEvent();

    [SerializeField]
    private GameObject m_GameOverPopup;

    private GameObject m_PlayerShip;

    void Start()
    {
        m_PlayerShip = GameObject.FindGameObjectWithTag(Tags.Player);

        m_PlayerShip.GetComponent<PlayerShipController>().m_ObjectHitEvent.AddListener(OnPlayerHit);
    }

    /// <summary>
    /// Revive player of he has lives
    /// </summary>
    /// <param name="newHP"></param>
    private void OnPlayerHit(int newHP)
    {
        // .. we don't need to check for newHP since the player has only 1 HP
        m_LivesCount--;

        if (m_LivesCount >= 0)
        {
            // .. Update lives UI
            m_LiveConsumedEvent.Invoke();

            // .. Revive
            StartCoroutine(ReviveAfter(1.5f));
        }
        else
        {
            // .. Fire the gameover event
            GameManager.Instance.m_GameOverEvent.Invoke();

            StartCoroutine(DisplayGameOverScreenAfter(1.2f));
        }
    }

	// Update is called once per frame
	private IEnumerator ReviveAfter(float duration)
    {
        yield return new WaitForSeconds(duration);

        // .. Reset player position
        m_PlayerShip.transform.position = Vector3.zero;

        // .. Enable player again
        m_PlayerShip.gameObject.SetActive(true);

        // .. Enter invulnerability state
        m_PlayerShip.GetComponent<MovingObjectCollision>().m_Invulnerable = true;
    }

    private IEnumerator DisplayGameOverScreenAfter(float duration)
    {
        yield return new WaitForSeconds(duration);

        // .. Play sound effect
        SoundManager.Instance.PlaySoundEffect(SoundEffectName.GAME_OVER_APPEAR);

        m_GameOverPopup.SetActive(true);
    }
}
