using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SequenceCardGame;
using System;
using System.Threading.Tasks;

public class InstructionUI : ScreenView
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

    public void OnBackBtnClk()
    {
        Utilities.WaitAsync(1500, () =>
        {
            UIManager.Instance.ChangeScreen(SceneName.SelectionMenu);
        });

        // UIAnimationManager.instance.Animate(AnimationMenu.GamePlay);
        GameStateManager.Instance.ChangeGameState(GameState.MainMenu);
    }

    void OnGameStateChange(GameState currentGameState)
    {

    }

}
