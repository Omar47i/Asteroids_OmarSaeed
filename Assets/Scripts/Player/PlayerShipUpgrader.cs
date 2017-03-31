using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShipUpgrader : MonoBehaviour {

    [SerializeField]
    private GameObject m_DefaultShipBody;

    [SerializeField]
    private GameObject m_AdvancedShipBody;

    [HideInInspector]
    public UnityEvent m_ShipUpgradeEvent = new UnityEvent();

    private bool m_Upgraded = false;

    void Start()
    {
        iTween.Init(gameObject);

        m_ShipUpgradeEvent.AddListener(OnUpgradeShip);
    }

    // .. Called when a double cannon pickup hits the player
	private void OnUpgradeShip()
    {
        if (m_Upgraded)
            return;
        else
            m_Upgraded = true;
        // .. Change the player ship sprite to the upgraded ship
        m_DefaultShipBody.SetActive(false);
        m_AdvancedShipBody.SetActive(true);
        
        GetComponent<PlayerShipController>().UpgradeShipCannon();

        // .. Play sound effect
        SoundManager.Instance.PlaySoundEffect(SoundEffectName.CANNON_UPGRADE);

        AnimateShip();
    }

    /// <summary>
    /// Animate ship on picking up the double cannon upgrade
    /// </summary>
    private void AnimateShip()
    {
        iTween.ShakePosition(gameObject, iTween.Hash(
                "x", 15f,
                "y", 15f,
                "time", .2f,
                "delay", 0f,
                "easeType", iTween.EaseType.easeOutCirc));
    }
}
