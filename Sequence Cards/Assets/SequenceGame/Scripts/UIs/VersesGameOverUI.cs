using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;
using UnityEngine.UI;

public class VersesGameOverUI : ScreenView
{
    public GameObject ball;
    public Text PlayerScoreTextGameOver;
    public Text AiScoreTextGameOver;
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

        // UIManager.Instance.ChangeScreen(SceneName.Verses);
        // UIAnimationManager.instance.Animate(AnimationMenu.VersesUI);
        GameStateManager.Instance.ChangeGameState(GameState.GamePlay);
        scoreManager.ResetScore();

        ScoreTimer.Instance.startInitialTimer();

    }
    public void OnGoHomeBtnClk()
    {

        UIManager.Instance.ChangeScreen(SceneName.MainMenu);
        // UIAnimationManager.instance.Animate(AnimationMenu.MainMenu);
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
    void ShowBestScore(int playerScore, int AIScore)
    {
        string[] text = PlayerScoreTextGameOver.text.Split(char.Parse(":"));
        PlayerScoreTextGameOver.text = text[0] + ":" + playerScore.ToString();
        text = AiScoreTextGameOver.text.Split(char.Parse(":"));
        AiScoreTextGameOver.text = text[0] + ":" + AIScore.ToString();
    }
}
