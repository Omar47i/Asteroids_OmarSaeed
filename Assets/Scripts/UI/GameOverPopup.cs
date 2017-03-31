using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPopup : MonoBehaviour {

    private int m_SlideOutHash;        // cash animator parameters

    private Animator m_Animator;       // cach animator component

    private bool m_ButtonPressed = false;

    private bool restartLevel = false;
    private bool returnToHome = false;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();

        m_SlideOutHash = Animator.StringToHash("SlideOut");
    }

    /// <summary>
    /// Return to main menu
    /// </summary>
    public void OnHome()
    {
        if (!m_ButtonPressed)
            m_ButtonPressed = true;
        else
            return;

        returnToHome = true;
        // .. Change game state to playing
        GameManager.Instance.m_GameState = GameState.MainMenu;

        // .. Play button click sound
        SoundManager.Instance.PlaySoundEffect(SoundEffectName.BUTTON_CLICK);

        // .. Slide out
        StartCoroutine(SlideOutDelayed(.2f));
    }

    /// <summary>
    /// Restart the gameplay scene
    /// </summary>
    public void OnRestart()
    {
        if (!m_ButtonPressed)
            m_ButtonPressed = true;
        else
            return;

        restartLevel = true;
        // .. Change game state to playing
        GameManager.Instance.m_GameState = GameState.Playing;

        // .. Play button click sound
        SoundManager.Instance.PlaySoundEffect(SoundEffectName.BUTTON_CLICK);

        // .. Slide out
        StartCoroutine(SlideOutDelayed(.29f));
    }

    IEnumerator SlideOutDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        m_Animator.SetTrigger(m_SlideOutHash);

        int sceneIndex = 0;

        if (restartLevel)
        {
            sceneIndex = 1;
        }
        else if (returnToHome)
        {
            sceneIndex = 0;
        }

        StartCoroutine(LoadSceneAfter(.5f, sceneIndex));
    }

    IEnumerator LoadSceneAfter(float delay, int sceneIndex)
    {
        yield return new WaitForSeconds(delay);

        // .. Play menu music
        SoundManager.Instance.PlayMusic(PlayingMusicType.MENU);

        // .. Load the scene requested scene
        SceneManager.LoadScene(sceneIndex);
    }
}
