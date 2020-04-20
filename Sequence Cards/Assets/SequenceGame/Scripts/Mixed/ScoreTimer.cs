using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SequenceCardGame;
using UnityEngine.UI;


public class ScoreTimer : MonoBehaviour
{
    public static ScoreTimer Instance;
    public ScoreManager scoreManager;
    public ModeManager modeManager;
    public float TotalTime = 30f;
    public bool TimeOn = false;
    public bool InitialTimeOn = false;
    Text textTimer;
    private void Awake()
    {
        Instance = this;
        textTimer = GetComponentInChildren<Text>();
    }
    private void Update()
    {
        if (InitialTimeOn)
        {
            TotalTime -= Time.deltaTime;
            textTimer.text = TotalTime.ToString("0");
            if (TotalTime < 0)
            {
                InitialTimeOn = false;
                StartTimer();

            }
        }
        if (TimeOn)
        {

            TotalTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(TotalTime / 60f);
            int seconds = Mathf.RoundToInt(TotalTime % 60f);

            string formatedSeconds = seconds.ToString();

            if (seconds == 60)
            {
                seconds = 0;
                minutes += 1;
            }

            //timer.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            textTimer.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            // if (TotalTime < 0)
            // {
            //     if (modeManager.GetCurrentMode() == GameMode.TimeTrails)
            //     {
            //         UIManager.Instance.ChangeScreen(SceneName.GameOver);
            //         GameStateManager.Instance.ChangeGameState(GameState.GameOver);
            //     }
            //     else
            //     {
            //         UIManager.Instance.ChangeScreen(SceneName.VersesGameOver);
            //         GameStateManager.Instance.ChangeGameState(GameState.GameOver);
            //     }
            //     scoreManager.GameOver();

            //     /* AudioManager.instance.Play("GameOver Sound");*/
            //     TotalTime = 30f;
            // }
        }
    }
    public void startInitialTimer()
    {
        TimeOn = false;
        TotalTime = 5f;
        InitialTimeOn = true;
    }
    public void StartTimer()
    {

        TotalTime = 30f;
        if (textTimer == null)
        {
            textTimer = GetComponentInChildren<Text>();

            Debug.Log(textTimer);
        }
        //StartCoroutine(TimeIncrement());
        TimeOn = true;

    }

}

