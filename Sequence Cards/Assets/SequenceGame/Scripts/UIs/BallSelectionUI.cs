using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SequenceCardGame;

public class BallSelectionUI : ScreenView
{


    private void OnEnable()
    {
        GameStateManager.onGameStateChange += OnGameStateChange;

    }
    private void OnDisable()
    {
        GameStateManager.onGameStateChange += OnGameStateChange;

    }
    public override void Show()
    {
        base.Show();
    }
    public override void Hide()
    {
        base.Hide();
    }

    /* public void OnPracticeBtnClk() {

         UIManager.Instance.ChangeScreen(SceneName.GamePlay);
         GameStateManager.Instance.ChangeGameState(GameState.GamePlay);

     }*/
    public void OnModeBtnClk()
    {
        UIManager.Instance.ChangeScreen(SceneName.GamePlay);
        GameStateManager.Instance.ChangeGameState(GameState.GamePlay);
    }
    public void OnModeVersesBtnClk()
    {

        // UIManager.Instance.ChangeScreen(SceneName.Verses);
        GameStateManager.Instance.ChangeGameState(GameState.GamePlay);

    }

    void OnGameStateChange(GameState currentGameState)
    {

    }

}
