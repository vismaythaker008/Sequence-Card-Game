using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;
using UnityEngine.UI;

public class GameOverUI : ScreenView
{
    public GameObject ball;
    public Text PlayerScoreTextGameOver;
    public Text PlayerHighScoreTextGameOver;
    public ScoreManager scoreManager;

    private void OnEnable()
    {
        // scoreManager.ShowBestScore += ShowBestScore;
        GameStateManager.onGameStateChange += OnGameStateChange;
    }
    private void OnDisable()
    {
        scoreManager.ScoreChanged -= ShowBestScore;
        GameStateManager.onGameStateChange -= OnGameStateChange;
    }
    private void Start()
    {

    }
    public override void Show()
    {

        base.Show();

    }
    public override void Hide()
    {
        base.Hide();
    }
    public void OnReplayBtnClk()
    {

        UIManager.Instance.ChangeScreen(SceneName.GamePlay);
        // UIAnimationManager.instance.Animate(AnimationMenu.GamePlay);
        GameStateManager.Instance.ChangeGameState(GameState.GamePlay);
        scoreManager.ResetScore();

        ScoreTimer.Instance.startInitialTimer();

    }
    public void OnGoHomeBtnClk()
    {

        UIManager.Instance.ChangeScreen(SceneName.SelectionMenu);
        // UIAnimationManager.instance.Animate(AnimationMenu.SelectionMenu);
        GameStateManager.Instance.ChangeGameState(GameState.MainMenu);
        scoreManager.ResetScore();
    }
    public void OnExitbtnClk()
    {

        Application.Quit();
    }
    void OnGameStateChange(GameState currentGameState)
    {
        /*if (currentGameState == GameState.RestartGamePlay)
        {
           
        }*/
    }
    void ShowBestScore(int playerScore, int HighScore)
    {
        string[] text = PlayerScoreTextGameOver.text.Split(char.Parse(":"));
        PlayerScoreTextGameOver.text = text[0] + ":" + playerScore.ToString();
        text = PlayerHighScoreTextGameOver.text.Split(char.Parse(":"));
        PlayerHighScoreTextGameOver.text = text[0] + ":" + HighScore.ToString();
    }
}
