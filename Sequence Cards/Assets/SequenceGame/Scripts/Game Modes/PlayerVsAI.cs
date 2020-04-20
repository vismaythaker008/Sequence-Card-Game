using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;

[CreateAssetMenu(menuName = "PlayerVsAI")]
public class PlayerVsAI : Mode
{
    private GameObject Timer;
    public ScoreManager scoreManager;
    ScoreTimer scoreTimer;
    public ModeManager modeManager;

    public void FindRunner()
    {
        Debug.Log("hello");
        // if (Timer == null)
        //     Timer = GameObject.Find("Timer Time Trail");
    }
    public override void OnEnter()
    {
        base.OnEnter();

        // FindRunner();
        // if (Timer.GetComponent<ScoreTimer>() == null)
        // {
        //     scoreTimer = Timer.AddComponent<ScoreTimer>();
        // }
        // scoreTimer.modeManager = modeManager;
        // scoreTimer.scoreManager = scoreManager;
        // scoreTimer.startInitialTimer();
        // Debug.Log("IN TimeTrail");
    }

    public override void OnExit()
    {
        gameMode = GameMode.None;
        // Destroy(scoreTimer);
        base.OnExit();
        //setScoreTimer();
        //remove componet

    }
}
