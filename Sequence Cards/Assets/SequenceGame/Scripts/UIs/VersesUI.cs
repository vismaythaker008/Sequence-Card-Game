using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SequenceCardGame;

public class VersesUI : ScreenView
{
    public ModeManager modeManager;
    public ScoreManager scoreManager;
    public Text PlayerScoreTextInVersesMode;
    public Text AIScoreTextInVersesMode;

    public GameObject AIPlayer;
    public GameObject PlayerBall;
    public GameObject AIBall;




    void Start()
    {

    }
    private void OnEnable()
    {
        // scoreManager.ScoreChanged += OnScoreChanged;
        GameStateManager.onGameStateChange += OnGameStateChange;
        // modeManager.ModeChanged += OnGameModeChange;
    }
    private void OnDisable()
    {
        scoreManager.ScoreChanged -= OnScoreChanged;
        GameStateManager.onGameStateChange -= OnGameStateChange;
        modeManager.ModeChanged -= OnGameModeChange;
    }
    public override void Show()
    {
        base.Show();
        Debug.Log("In Verses mode");
    }
    public override void Hide()
    {
        base.Hide();
        GameStateManager.onGameStateChange -= OnGameStateChange;
    }
    public void OnResumeBtnClk()
    {

        // UIManager.Instance.ChangeScreen(SceneName.Verses);
        //UIAnimationManager.instance.Animate(AnimationMenu.VersesUI);
        GameStateManager.Instance.ChangeGameState(GameState.GamePlay);

    }
    public void OnRestartBtnClk()
    {

        // UIManager.Instance.ChangeScreen(SceneName.Verses);
        //UIAnimationManager.instance.Animate(AnimationMenu.VersesUI);
        GameStateManager.Instance.ChangeGameState(GameState.GamePlay);
        scoreManager.ResetScore();


    }
    public void OnMainMenuBtnClk()
    {

        UIManager.Instance.ChangeScreen(SceneName.SelectionMenu);
        //UIAnimationManager.instance.Animate(AnimationMenu.MainMenu);
        GameStateManager.Instance.ChangeGameState(GameState.MainMenu);
        scoreManager.ResetScore();


    }

    public void OnGameStateChange(GameState currentGameState)
    {

        if (currentGameState == GameState.GamePlay)
        {
            Time.timeScale = 1;
        }
    }
    public void OnPauseBtnClk()
    {

        UIManager.Instance.ChangeScreen(SceneName.PausedGamePlay);
        // UIAnimationManager.instance.Animate(AnimationMenu.PausedGamePlay);
        GameStateManager.Instance.ChangeGameState(GameState.PausedGamePlay);
        Time.timeScale = 0;

    }

    public void OnGameModeChange(GameMode currentMode)
    {
        Debug.Log(currentMode);
        Debug.Log("Mode Change");
        // if (currentMode == GameMode.Verses)
        // {
        //     AIPlayer.SetActive(true);

        //     ScoreTimer.Instance.startInitialTimer();

        //     // GameManagerScript.instance.AIPlayerEnable();
        //     //Add and remove components as requires
        //     Debug.Log("In Gameplay -> Verses");
        // }
    }
    public void OnScoreChanged(int playerScore, int AIScore)
    {

        string[] text = PlayerScoreTextInVersesMode.text.Split(char.Parse(":"));
        PlayerScoreTextInVersesMode.text = text[0] + ":" + playerScore.ToString();
        text = AIScoreTextInVersesMode.text.Split(char.Parse(":"));
        AIScoreTextInVersesMode.text = text[0] + ":" + AIScore.ToString();

    }

}