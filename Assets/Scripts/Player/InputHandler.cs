using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    // .. Cach input variables
    [HideInInspector]
    public float m_Horizontal = 0f;

    [HideInInspector]
    public float m_Vertical = 0f;

    [HideInInspector]
    public bool m_Fire = false;

    // .. Define the commands that can be issued by the player
    private ICommand moveTurnCommand;
    private ICommand fireCommand;
    private ICommand nullCommand;

    [SerializeField]
    private GameObject m_MobileControlsCanvas;
    private MobileInputHandler m_MobileInput;   // get axis for mobile controls

    void Awake()
    {
        // .. Initialize the commands that we can issue to our spaceship
        moveTurnCommand = new MoveAndTurnCommand();
        fireCommand = new FireCommand();
        nullCommand = new NullCommand();

        // .. TODO: Instanitate mobile controls canvas and get references to it
#if !UNITY_EDITOR && !UNITY_STANDALONE
        Instantiate(m_MobileControlsCanvas);
#endif
    }

    void Start()
    {
        // .. TODO: Get refeernec to the axis simulator script
#if !UNITY_EDITOR && !UNITY_STANDALONE
        m_MobileInput = GameObject.FindGameObjectWithTag(Tags.MobileControls).GetComponent<MobileInputHandler>();
#endif
    }

    void Update()
    {
        m_Fire = false;

#if UNITY_EDITOR || UNITY_STANDALONE     // Check if we are running either in the Unity editor or in a standalone build.
        GetKeyboardInput();
#else                                    //Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
        GetTouchInput();
#endif
        ICommand movementCommand = HandleMovementInput();   // Get movement/turning input if exists
        ICommand fireCommand = HandleFireInput();           // Get fire input if exists

        // Execute the commands issued by the player
        movementCommand.Execute(gameObject);
        fireCommand.Execute(gameObject);
    }

    private void GetKeyboardInput()
    {
        m_Horizontal = Input.GetAxisRaw("Horizontal");
        m_Vertical = Input.GetAxis("Vertical");
        m_Fire = Input.GetKey(KeyCode.Space);
    }

    private void GetTouchInput()
    {
        m_Vertical = m_MobileInput.m_VerticalAxis;
        m_Horizontal = m_MobileInput.m_HorizontalAxis;
        m_Fire = m_MobileInput.m_FireDown;
    }

    /// <summary>
    /// Check for spaceship movement input
    /// </summary>
    /// <returns>The command issued by the user or null</returns>
    private ICommand HandleMovementInput()
    {
        if (m_Vertical != 0f || m_Horizontal != 0f)
            return moveTurnCommand;

        return nullCommand;
    }

    /// <summary>
    /// Check for spaceship movement input
    /// </summary>
    /// <returns>The command issued by the user or null</returns>
    private ICommand HandleFireInput()
    {
        if (m_Fire) return fireCommand;

        return nullCommand;
    }
}