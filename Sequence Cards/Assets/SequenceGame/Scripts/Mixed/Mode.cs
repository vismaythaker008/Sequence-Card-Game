using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;

public class Mode : ScriptableObject
{
    public GameMode gameMode = GameMode.None;
    public virtual void OnEnter()
    {

        //Debug.Log("OnEnter");
    }
    public virtual void OnExit()
    {

        //Debug.Log("OnExit");
    }
}
