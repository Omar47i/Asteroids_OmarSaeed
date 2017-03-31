using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundControls : MonoBehaviour {

    public static int IsMusicMuted;
    public static int IsSoundEffectsMuted;

    public GameObject soundButton;
    public GameObject musicButton;

    private bool ignoreMusicToggle = true;
    private bool ignoreSoundsToggle = true;
    void Awake()
    {
        // .. Get the previously saved music and sound effects states
        IsMusicMuted = PlayerSettings.GetMusicState();
        IsSoundEffectsMuted = PlayerSettings.GetSoundEffectsState();
    }

    void Start()
    {
        if (IsMusicMuted == 1)
            musicButton.GetComponent<Toggle>().isOn = true;
        else
            musicButton.GetComponent<Toggle>().isOn = false;

        if (IsSoundEffectsMuted == 1)
            soundButton.GetComponent<Toggle>().isOn = false;
        else
            soundButton.GetComponent<Toggle>().isOn = true;
    }

    public void ToggleMusic()
    {
        if (ignoreMusicToggle)
        {
            ignoreMusicToggle = false;
            return;
        }
        // toggle music/sound only when user click on the sound/music button,
        // so we wait for a time to avoid calling the toggle function when we set it manually from PlayerPrefs
        ToggleMuteMusic();
	}

	public void ToggleSoundEffect()
    {
        if (ignoreSoundsToggle)
        {
            ignoreSoundsToggle = false;
            return;
        }

        ToggleMuteSoundEffects();
	}

    /// <summary>
    /// Toggle sound effects state to On/Off
    /// </summary>
    public void ToggleMuteSoundEffects()
    {
        if (IsSoundEffectsMuted == 0)
        {
            IsSoundEffectsMuted = 1;
            SoundManager.IsSoundEffectsMuted = IsSoundEffectsMuted;
            SoundManager.Instance.audioSrc.Stop();
        }
        else
        {
            IsSoundEffectsMuted = 0;
            SoundManager.IsSoundEffectsMuted = IsSoundEffectsMuted;
        }

        // .. Save the sound effects state
        PlayerSettings.SetSoundEffectsState(IsSoundEffectsMuted);
    }

    /// <summary>
    /// Toggle music state to On/Off
    /// </summary>
    public void ToggleMuteMusic()
    {
        if (IsMusicMuted == 0)
        {
            SoundManager.musicType = PlayingMusicType.NONE;
            IsMusicMuted = 1;
            SoundManager.IsMusicMuted = IsMusicMuted;
            SoundManager.Instance.musicAudioSrc.Stop();
        }
        else
        {
            IsMusicMuted = 0;
            SoundManager.IsMusicMuted = IsMusicMuted;
            SoundManager.Instance.PlayMenuMusic();
        }

        // save the new music state
        PlayerSettings.SetMusicState(IsMusicMuted);
    }
}
