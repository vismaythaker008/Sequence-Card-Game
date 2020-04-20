using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;
public class BallStateManager : MonoBehaviour
{

    BallState currentBallState = BallState.BallIdle;

    public delegate void OnBallStateChange(BallState ballState);
    public static event OnBallStateChange onBallStateChange;


    public void ChangeBallState(BallState ballState)
    {
        if (currentBallState != ballState)
        {
            currentBallState = ballState;
            if (onBallStateChange != null)
            {
                onBallStateChange(currentBallState);
            }
        }

    }
    public bool CheckBallState(BallState ballState)
    {
        return currentBallState == ballState;
    }
}

