using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SequenceCardGame;

public class PausedMenuUI : ScreenView
{

    public ScoreManager scoreManager;
    public ModeManager modeManager;
    private void OnEnable()
    {
        GameStateManager.onGameStateChange += OnGameStateChange;
    }
    private void OnDisable()
    {
        GameStateManager.onGameStateChange -= OnGameStateChange;
    }
    public override void Show()
    {
        base.Show();
        Debug.Log("In pause");

    }
    public override void Hide()
    {
        base.Hide();
    }
    public void OnResumeBtnClk()
    {
        // if (modeManager.GetCurrentMode() == GameMode.Verses)
        // {
        //     UIManager.Instance.ChangeScreen(SceneName.Verses);
        //     // UIAnimationManager.instance.Animate(AnimationMenu.VersesUI);

        // }
        // else
        // {
        //     UIManager.Instance.ChangeScreen(SceneName.GamePlay);
        //     // UIAnimationManager.instance.Animate(AnimationMenu.GamePlay);
        // }
        // GameStateManager.Instance.ChangeGameState(GameState.GamePlay);

    }
    public void OnRestartBtnClk()
    {
        /*if (modeManager.GetCurrentMode() == GameMode.Verses)
        {
            UIManager.Instance.ChangeScreen(SceneName.Verses);
            // UIAnimationManager.instance.Animate(AnimationMenu.VersesUI);
           
        }
        else
        {
            // UIAnimationManager.instance.Animate(AnimationMenu.GamePlay);
            UIManager.Instance.ChangeScreen(SceneName.GamePlay);
        }
        GameStateManager.Instance.ChangeGameState(GameState.GamePlay);
        scoreManager.ResetScore();*/

        if (ScoreTimer.Instance != null)
            ScoreTimer.Instance.startInitialTimer();
    }
    public void OnMainMenuBtnClk()
    {
        /*if (modeManager.GetCurrentMode() == GameMode.Verses)
        {
            GameManagerScript.instance.AIPlayerDisable();
            AiBall.Restart();
        }
        UIManager.Instance.ChangeScreen(SceneName.SelectionMenu);
        // UIAnimationManager.instance.Animate(AnimationMenu.MainMenu);
        GameStateManager.Instance.ChangeGameState(GameState.MainMenu);
        scoreManager.ResetScore();
        playerBall.Restart();*/

    }
    void OnGameStateChange(GameState currentGameState)
    {

    }
}
