using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using SequenceCardGame;

[CreateAssetMenu(menuName = "ScoreManager")]
public class ScoreManager : ScriptableObject
{
    public delegate void OnScoreChange(int playerScore, int aiScore);
    public event OnScoreChange ScoreChanged;
    public delegate void OnGameOver(int playerScore, int bestScore);
    public event OnGameOver ShowBestScore;

    public int playerScore;
    public int AIScore;
    public int BestScore;
    public ModeManager modeManager;

    private void Start()
    {
        playerScore = 0;
        AIScore = 0;
        BestScore = SaveLoadManager.LoadPlayerData();

    }
    public void ScoreIncrement(int PlayerScore, int aiScore)
    {
        playerScore += PlayerScore;
        AIScore += aiScore;
        ScoreChanged(playerScore, AIScore);
    }
    public void ScoreDecrement(int PlayerScore, int aiScore)
    {
        playerScore -= PlayerScore;
        AIScore -= aiScore;
        ScoreChanged(playerScore, AIScore);
    }
    public void ResetScore()
    {
        Debug.Log("Reseting Score NOw");
        playerScore = 0;
        AIScore = 0;
        ScoreChanged(playerScore, AIScore);
    }
    public void GameOver()
    {
        // if (playerScore > BestScore)
        // {
        //     SaveLoadManager.SavePlayerData(new Data(playerScore));
        //     BestScore = playerScore;
        // }
        // Debug.Log("before score" + playerScore);
        // if (modeManager.GetCurrentMode() == GameMode.Verses)
        // {
        //     ShowBestScore(playerScore, AIScore);
        // }
        // else
        // {
        //     ShowBestScore(playerScore, BestScore);
        // }

        Debug.Log("after score" + playerScore);

    }
}
