using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAnimator : MonoBehaviour
{
    [SerializeField]
    private Transform m_LeftBeam;            // Reference to the left beam transform component

    [SerializeField]
    private Transform m_RightBeam;          // Reference to the right beam transform component

    //[SerializeField]
    //private float m_AnimationSpeed;         // starting and stoping beam animation speed

    private InputHandler inputScript;       // to get the movement amount
    //private float m_Scale = 0f;

    void Start()
    {
        // .. Listen to the spaceship movement event to animate the spaceship properly
        inputScript = transform.parent.GetComponent<InputHandler>();
    }

    void Update()
    {
        m_LeftBeam.localScale = new Vector3(1f, Mathf.Abs(inputScript.m_Vertical), 1f);
        m_RightBeam.localScale = new Vector3(1f, Mathf.Abs(inputScript.m_Vertical), 1f);
    }

}
