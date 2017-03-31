using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonUpgradePickup : MonoBehaviour
{
    void Awake()
    {
        iTween.Init(gameObject);     // initialize tween engine for this object to avoid hiccups

        StartingAnimation();         // play the starting animation
    }

    void Start()
    { 
        // .. play pickup animation
        StartingAnimation();
    }

    private void StartingAnimation()
    {
        iTween.ScaleAdd(gameObject, iTween.Hash(
                "amount", new Vector3(.7f, .7f, .7f),
                "time", .15f,
                "looptype", iTween.LoopType.pingPong,
                "easeType", iTween.EaseType.linear));
    }
}
