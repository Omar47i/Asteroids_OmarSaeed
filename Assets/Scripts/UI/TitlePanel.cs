using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitlePanel : MonoBehaviour
{
    [SerializeField]
    private Text m_BestScoreText;      // reference to the best score text component

    private Animator m_Animator;       // cach animator component

    private int m_SlideOutHash;        // cash animator parameters

    private bool m_PlayButtonPressed = false;
    void Awake()
    {
        m_Animator = GetComponent<Animator>();

        m_SlideOutHash = Animator.StringToHash("SlideOut");
    }

    void Start()
    {
        // .. Set the best score value
        m_BestScoreText.text = PlayerSettings.GetBestScore().ToString();
    }

    public void OnPlay()
    {
        if (!m_PlayButtonPressed)
            m_PlayButtonPressed = true;
        else
            return;

        // .. Play sfx
        SoundManager.Instance.PlaySoundEffect(SoundEffectName.BUTTON_CLICK);

        // .. Change game state to playing
        GameManager.Instance.m_GameState = GameState.Playing;

        // .. Slide out
        StartCoroutine(SlideOutDelayed(.2f));
    }  

    IEnumerator SlideOutDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        m_Animator.SetTrigger(m_SlideOutHash);

        StartCoroutine(LoadSceneAfter(.5f));
    }

    IEnumerator LoadSceneAfter(float delay)
    {
        yield return new WaitForSeconds(delay);

        // .. Play in game music
        SoundManager.Instance.PlayMusic(PlayingMusicType.IN_GAME);

        // .. Load the gameplay level
        SceneManager.LoadScene(1);
    }
}
