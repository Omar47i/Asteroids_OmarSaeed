using UnityEngine;
using UnityEngine.UI;

// .. Store all sound effect names to easily refere to them from outside the class scope
public enum SoundEffectName
{
    BUTTON_CLICK,
    FIRE_PROJECTILE,
    EXPLODE,
    GAME_OVER_APPEAR,
    NEW_HIGH_SCORE,
    CANNON_UPGRADE,
    ENEMY_SPACESHIP_APPEAR,
};

public enum PlayingMusicType
{
    NONE = -1,
    MENU = 0,
    IN_GAME = 1,
};

[System.Serializable]
public struct SoundEffect
{
    public SoundEffectName effectName;
    public AudioClip effectAudio;
}

//Required components of this class
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static int IsMusicMuted;           // flags to indicate if the sound or music is muted
    public static int IsSoundEffectsMuted;

    public static PlayingMusicType musicType;

    public AudioClip[] MusicAudios;

    [Header("Please set sfx names in SoundEffectName enum in SoundManager.cs.")]
    public SoundEffect[] soundEffectAudios;

    public static SoundManager Instance = null;

    [HideInInspector]
    public AudioSource audioSrc;      // the attached audio source

    [HideInInspector]
    public AudioSource musicAudioSrc;  // the attached audio source for playing music
    private float startingVol;         // store the starting volume of the audio source

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)   // duplicate
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);     // make it survive scene changes

        audioSrc = GetComponent<AudioSource>();
        musicAudioSrc = transform.GetChild(0).GetComponent<AudioSource>();
        musicType = PlayingMusicType.NONE;   

        startingVol = audioSrc.volume;
    }

    void Start()
    {
        IsMusicMuted = SoundControls.IsMusicMuted;
        IsSoundEffectsMuted = SoundControls.IsSoundEffectsMuted;

        PlayMenuMusic();
    }

    public void PlaySoundEffect(SoundEffectName effectName, bool changePitch = false, float vol = -1)
    {
        if (IsSoundEffectsMuted == 0)   // if the sound effects aren't muted, play it
        {
            for (int i = 0; i < soundEffectAudios.Length; i++)
            {
                if (soundEffectAudios[i].effectName == effectName)
                {
                    // set the audio source volume if specified, if not set the default volume
                    vol = (vol != -1f) ? vol : startingVol;

                    if (changePitch)
                    {
                        float newPitch = Random.Range(.95f, 1.05f);
                        audioSrc.pitch = newPitch;
                    }
                    else
                    {
                        audioSrc.pitch = 1f;
                    }
                   
                    audioSrc.PlayOneShot(soundEffectAudios[i].effectAudio, vol);
                    break;
                }
            }
        }
    }

    public void PlayMenuMusic()
    {
        if (musicType != PlayingMusicType.MENU)
        {
            if ((MusicAudios.Length > 0) && (IsMusicMuted == 0))
            {
                musicAudioSrc.pitch = 1f;
                musicAudioSrc.clip = MusicAudios[(int)PlayingMusicType.MENU];
                musicAudioSrc.loop = true;
                musicAudioSrc.Play();
                musicType = PlayingMusicType.MENU;
            }
        }
    }

    public void PlayMusic(PlayingMusicType type)
    {
        if ((MusicAudios.Length > 0) && (IsMusicMuted == 0))
        {
            if (type == musicType)
                return;

            musicType = type;
            musicAudioSrc.pitch = 1f;
            musicAudioSrc.clip = MusicAudios[(int)type];
            musicAudioSrc.loop = true;
            musicAudioSrc.Play();
        }
    }

    /// <summary>
    /// Mute the audio source
    /// </summary>
    public void StopMusic()
    {
        musicType = PlayingMusicType.NONE;
        musicAudioSrc.Stop();
        //audioSrc.Stop();
    }
}