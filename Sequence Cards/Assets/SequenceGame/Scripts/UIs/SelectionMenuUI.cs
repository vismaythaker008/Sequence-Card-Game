using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SequenceCardGame;

public class SelectionMenuUI : ScreenView
{
    public GameplayUI gameplayUI;

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
    public void OnModeBtnClk(string mode)
    {
        switch (mode)
        {
            case "Learn To Play":
                Utilities.WaitAsync(1500, () =>
                {
                    UIManager.Instance.ChangeScreen(SceneName.Instructions);
                });

                // UIAnimationManager.instance.Animate(AnimationMenu.GamePlay);
                GameStateManager.Instance.ChangeGameState(GameState.MainMenu);
                break;
            case "Play VS AI":
                UIManager.Instance.ChangeScreen(SceneName.PlayerSelectionMenu);
                gameplayUI.setPlayerNames("You", "AI", "AI");
                // UIAnimationManager.instance.Animate(AnimationMenu.GamePlay);
                GameStateManager.Instance.ChangeGameState(GameState.MainMenu);
                break;
            case "Play Online":
                break;
        }

    }


    void OnGameStateChange(GameState currentGameState)
    {

    }

}
