using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerSpaceship : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer spRendDefault;   // renderer for regular ship body

    [SerializeField]
    private SpriteRenderer spRendAdvanced;  // renderer for advanced ship body

    private bool stopFlickering = false;

	void Start()
    {
        GetComponent<MovingObjectCollision>().PlayerInvulnerableEvent.AddListener(OnInvulnerable);
    }

    /// <summary>
    /// Called when player is in invulnerable state
    /// </summary>
    /// <param name="duration">duration of flickering</param>
    private void OnInvulnerable(float duration)
    {
        stopFlickering = false;

        StartCoroutine(Flicker());

        StartCoroutine(Invulnerable(duration));
    }

    IEnumerator Invulnerable(float duration)
    {
        yield return new WaitForSeconds(duration);

        stopFlickering = true;

        GetComponent<MovingObjectCollision>().m_Invulnerable = false;
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            if (stopFlickering)
            {
                spRendDefault.enabled = true;
                spRendAdvanced.enabled = true;
                yield break;
            }

            spRendDefault.enabled = !spRendDefault.enabled;
            spRendAdvanced.enabled = !spRendAdvanced.enabled; ;
            yield return new WaitForSeconds(.1f);

            StopCoroutine(Flicker());
        }
    }
}
